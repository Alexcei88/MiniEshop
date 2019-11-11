using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MiniEshop.Domain.DTO;
using MiniEshop.Services;

namespace UploadFilesServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController
        : ControllerBase
    {
        private readonly IFileService _fileService;

        private readonly IMapper _mapper;

        public UploadController(IFileService fileService,
             IMapper mapper)
        {
            _fileService = fileService;
            _mapper = mapper;
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

                    return Ok(_mapper.Map<FileLinkDTO>(savingImages));
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
        public async Task<IActionResult> Delete([FromQuery]Guid id)
        {
            string deletedPath = await _fileService.DeleteImageAsync(id);
            return Ok(new { deletedPath });
        }
    }
}