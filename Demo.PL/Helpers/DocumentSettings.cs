using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Demo.PL.Helpers
{
    public static class DocumentSettings
    {
        public static string UploadFile(IFormFile file, string FolderName)
        {
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName);
            string FileName = $"{Guid.NewGuid()}{file.FileName}";
            string PathFile = Path.Combine(FolderPath, FileName);
            using var Fs = new FileStream(PathFile, FileMode.Create);
            file.CopyTo(Fs);
            return PathFile;
        }
       


        public static void DeleteFile(string fileName , string FolderName)
        {
            var pathFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName, FolderName);

            if(File.Exists(pathFile))
            {
                File.Delete(pathFile);
            }
        }
    }
}
