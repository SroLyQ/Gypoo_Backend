using MongoDB.Driver;
using MongoDB.Bson;
using System.Security.Cryptography;
using GypooWebAPI.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Principal;

namespace GypooWebAPI.Services
{
    public class UserService
    {
        private MongoDBService _mongoDBservice;
        private IMongoCollection<User> _userCollection;
        private readonly IConfiguration _configuration;
        public UserService(MongoDBService mongoDBService, IConfiguration configuration)
        {
            _mongoDBservice = mongoDBService;
            _userCollection = _mongoDBservice._userCollection;
            _configuration = configuration;
        }
        private void createPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var loginHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return loginHash.SequenceEqual(passwordHash);
            }
        }
        private TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:JWTSecretKey").Value)),
            };
        }
        private string CreateToken(User user)
        {

            List<Claim> claims = new List<Claim>{
                new Claim("username", user.username),
                new Claim("role", "AdminKodHod"),
                new Claim("userID",user.Id),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:JWTSecretKey").Value
            ));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: cred
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        public async Task<UserNoPW> GetUserByIdAsync(string id)
        {
            User users = await _userCollection.Find((_user => _user.Id == id)).SingleAsync();
            UserNoPW userNoPW = (UserNoPW)users;
            return userNoPW;
        }
        public async Task<UserNoPW> registerAsync(UserDTO user)
        {
            createPasswordHash(user.password, out byte[] passwordHash, out byte[] passwordSalt);
            var _existUser = await _userCollection.Find(_user => _user.username == user.username).SingleOrDefaultAsync();
            if (_existUser != null)
            {
                return null;
            }
            var _user = new User();

            _user.username = user.username;
            _user.PasswordHash = passwordHash;
            _user.PasswordSalt = passwordSalt;

            string token = CreateToken(_user);

            _user.token = token;
            await _userCollection.InsertOneAsync(_user);
            UserNoPW _resUser = (UserNoPW)_user;
            return _resUser;
        }
        public async Task<string> loginAsync(UserDTO user)
        {
            var _user = await _userCollection.Find(_user => _user.username == user.username).SingleOrDefaultAsync();
            if (_user == null)
            {
                return "UsernameFalse";
            }
            if (VerifyPasswordHash(user.password, _user.PasswordHash, _user.PasswordSalt))
            {
                string token = CreateToken(_user);
                var update = Builders<User>.Update.Set("token", token);
                var res = await _userCollection.UpdateOneAsync(_user => _user.username == user.username, update);
                return token;
            }
            else
            {
                return "false";
            }
        }
        public bool validateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameter = GetTokenValidationParameters();

            SecurityToken validatedToken;
            try
            {
                IPrincipal principal = tokenHandler.ValidateToken(token, validationParameter, out validatedToken);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        public async Task<string> getUsername(string id)
        {
            User user = await _userCollection.Find(_user => _user.Id == id).SingleAsync();
            if (user == null)
            {
                return "Not Found";
            }
            return user.username;
        }
        public async Task<UserNoPW> updateUser(string id, UserUpdate userData)
        {
            var option = new FindOneAndUpdateOptions<User, User>
            {
                IsUpsert = false,
                ReturnDocument = ReturnDocument.After
            };
            var update = Builders<User>.Update.Set("name", userData.name).Set("surname", userData.surname).Set("email", userData.email);
            User user = await _userCollection.FindOneAndUpdateAsync<User>(_user => _user.Id == id, update, option);
            UserNoPW _user = (UserNoPW)user;
            return _user;
        }
    }
}