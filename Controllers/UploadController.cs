using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniEshop.DAL;
using MiniEshop.Services;

namespace UploadFilesServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController
        : ControllerBase
    {
        private readonly IFileService _fileService;

        public UploadController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var file = Request.Form.Files[0];

                if (file.Length > 0)
                {
                    var savingImages = await _fileService.UploadImageAsync(file.OpenReadStream()
                        , ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"'));

                    return Ok(new { dbPath = savingImages.path, newImage = savingImages.isNewImage });
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

        [HttpDelete]
        public IActionResult Delete([FromQuery]string dbPath)
        {
            string deletedPath = _fileService.DeleteImage(dbPath);
            return Ok(new { deletedPath });
        }
    }
}