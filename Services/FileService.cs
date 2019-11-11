using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using MiniEshop.DAL;
using MiniEshop.Domain;
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
        private readonly IFileServiceRepository _fileRepository;

        public FileService(IFileServiceRepository fileRepository)
        {
            _staticImagesFolder = Path.Combine("StaticFiles", "Images");
            _fileRepository = fileRepository;
        }

        public async Task<FileLink> UploadImageAsync(Stream stream, string sourceFileName)
        {
            // 1. Сохраняем во временную папку файл
            string tempDirectory = Path.Combine(_staticImagesFolder, "TempImages");
            string tempFileName = Path.Combine(tempDirectory, sourceFileName);
            tempFileName = await SaveFileStream(stream, tempFileName);
            // 2. высчитываем md5 hash, это будет наше название файла
            string fileName = ComputeMD5Hash(tempFileName);
            // 3. проверяем сущноствование файла в хранилище
            var dbPath = Path.Combine(_staticImagesFolder, fileName);
            if (await _fileRepository.IsExistFileAsync(dbPath))
            {
                return await _fileRepository.SaveFileAsync(dbPath);
            }
            else
            {
                System.IO.File.Move(tempFileName, dbPath, true);
                return await _fileRepository.SaveFileAsync(dbPath);
            }
        }

        public async Task<string> DeleteImageAsync(Guid id)
        {
            var link = await _fileRepository.DeleteFileAsync(id);
            if (!await _fileRepository.IsExistFileAsync(link.FileUrl))
            {
                if (System.IO.File.Exists(link.FileUrl))
                    System.IO.File.Delete(link.FileUrl);
            }
            return link.FileUrl;
        }

        private async Task<string> SaveFileStream(Stream file, string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return fileName;
        }

        private string ComputeMD5Hash(string fileName)
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

        public async Task<string> GetImagePathAsync(Guid id)
        {
            return (await _fileRepository.GetFileLinkAsync(id))?.FileUrl;
        }
    }
}
