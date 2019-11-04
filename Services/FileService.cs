using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using MiniEshop.DAL;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MiniEshop.Services
{
    public class FileService
        : IFileService
    {
        private readonly string _staticImagesFolder;
        private readonly IEshopRepository _eshopRepository;

        public FileService(IEshopRepository eshopRepository)
        {
            _staticImagesFolder = Path.Combine("StaticFiles", "Images");
            _eshopRepository = eshopRepository;
        }

        public async Task<(string path, bool isNewImage)> UploadImageAsync(Stream stream, string sourceFileName)
        {
            // 1. Сохраняем во временную папку файл
            string tempDirectory = Path.Combine(_staticImagesFolder, "TempImages");
            string tempFileName = Path.Combine(tempDirectory, sourceFileName);
            tempFileName = await saveFileStream(stream, tempFileName);
            // 2. высчитываем md5 hash, это будет наше название файла
            string fileName = computeMD5Hash(tempFileName);
            // 3. проверяем сущноствование файла в хранилище
            var dbPath = Path.Combine(_staticImagesFolder, fileName);
            if (await _eshopRepository.IsExistGoodWithImage(dbPath))
            {
                return (path: dbPath, isNewImage: false);
            }
            else
            {
                System.IO.File.Move(tempFileName, dbPath, true);
                return (path: dbPath, isNewImage: true);
            }
        }

        public string DeleteImage(string dbPath)
        {
            if (System.IO.File.Exists(dbPath))
                System.IO.File.Delete(dbPath);
            return dbPath;
        }

        private async Task<string> saveFileStream(Stream file, string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return fileName;
        }

        private string computeMD5Hash(string fileName)
        {
            using System.Security.Cryptography.SHA1 md5 = System.Security.Cryptography.SHA1.Create();
            using var inputStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            byte[] hashBytes = md5.ComputeHash(inputStream);
            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }

            return sb.ToString() + Path.GetExtension(fileName);
        }

    }
}
