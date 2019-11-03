using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UploadFilesServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController
        : ControllerBase
    {
        private readonly string _staticImagesFolder;

        public UploadController()
        {
            _staticImagesFolder = Path.Combine("StaticFiles", "Images");

        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), _staticImagesFolder);

                if (file.Length > 0)
                {
                    string tempFile = await saveFileStream(file, Path.Combine(_staticImagesFolder, "TempImages"));
                    string fileName = computeMD5Hash(tempFile);
                    DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(_staticImagesFolder);
                    FileInfo[] filesInDir = hdDirectoryInWhichToSearch.GetFiles(fileName);
                    var dbPath = Path.Combine(_staticImagesFolder, fileName);
                    if (filesInDir.Any())
                    {
                        bool findEqual = false;
                        foreach(var fileDir in filesInDir)
                        {
                            if(FilesAreEqual(fileDir.FullName, tempFile))
                            {
                                findEqual = true;
                                break;
                            }
                        }
                        if(!findEqual)
                            System.IO.File.Move(tempFile, dbPath);
                    }
                    else
                    {
                        System.IO.File.Move(tempFile, dbPath);
                    }
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private async Task<string> saveFileStream(IFormFile file, string directory)
        {
            var fileName = Path.Combine(directory, ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"'));
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

            return sb.ToString() + "." + Path.GetExtension(fileName);
        }


        bool FilesAreEqual(string f1, string f2)
        {
            // get file length and make sure lengths are identical
            long length = new FileInfo(f1).Length;
            if (length != new FileInfo(f2).Length)
                return false;

            // open both for reading
            using (FileStream stream1 = System.IO.File.OpenRead(f1))
            using (FileStream stream2 = System.IO.File.OpenRead(f2))
            {
                // compare content for equality
                int b1, b2;
                while (length-- > 0)
                {
                    b1 = stream1.ReadByte();
                    b2 = stream2.ReadByte();
                    if (b1 != b2)
                        return false;
                }
            }

            return true;
        }
    }
}