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
                new Claim("Username", user.Username),
                new Claim("Role", "AdminKodHod")
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

        public async Task<List<User>> GetUsersAsync()
        {
            return await _userCollection.Find(new BsonDocument()).ToListAsync();
        }
        public async Task<User> registerAsync(UserDTO user)
        {
            createPasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var _user = new User();

            _user.Username = user.Username;
            _user.PasswordHash = passwordHash;
            _user.PasswordSalt = passwordSalt;
            await _userCollection.InsertOneAsync(_user);

            return _user;
        }

        public async Task<string> loginAsync(UserDTO user)
        {
            var _user = await _userCollection.Find(_user => _user.Username == user.Username).SingleOrDefaultAsync();
            if (_user == null)
            {
                return "UsernameFalse";
            }
            if (VerifyPasswordHash(user.Password, _user.PasswordHash, _user.PasswordSalt))
            {
                string token = CreateToken(_user);
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
    }
}