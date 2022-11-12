namespace GypooWebAPI.Services
{
    public class UploadService
    {
        private readonly IConfiguration _configuration;

        private readonly string imgDir;
        public UploadService(IConfiguration configuration)
        {
            _configuration = configuration;
            imgDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
        }

        public async Task<List<string>> uploadPictures(List<IFormFile> files)
        {
            List<string> imgPath = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var fileName = Path.GetFileName(formFile.FileName);
                    var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                    var fileExtension = Path.GetExtension(fileName);
                    var newFileName = String.Concat(myUniqueFileName, fileExtension);
                    if (!Directory.Exists(imgDir))
                    {
                        Directory.CreateDirectory(imgDir);
                    }

                    try
                    {
                        using (var stream = System.IO.File.Create(imgDir + $@"\{newFileName}"))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                        imgPath.Add(newFileName);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return new List<string>() { "IO ERROR" };
                    }
                }
            }
            return imgPath;
        }
    }
}