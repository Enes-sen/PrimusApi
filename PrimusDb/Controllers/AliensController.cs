using Business.Abstract;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PrimusDb.helpers;

namespace PrimusDb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AliensController : ControllerBase
    {
        private readonly IAlienService _alienService;
        private readonly IOptions<CloudinarySettings> _cloudConfig;
        private readonly Cloudinary _cloudinary;
        public AliensController(IAlienService alienService, IOptions<CloudinarySettings> options)
        {
            _alienService = alienService;
            _cloudConfig = options;

            Account account = new()
            {
                Cloud = _cloudConfig.Value.CloudName,
                ApiKey = _cloudConfig.Value.ApiKey,
                ApiSecret = _cloudConfig.Value.ApiSecret,
            };
            _cloudinary = new Cloudinary(account);
        }


        [HttpPost("add")]
        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Add([FromForm] Alien alien)
        {
            if (alien == null)
            {
                return BadRequest("Geçersiz alien Verisi");
            }

            if (alien.FormFile == null || alien.FormFile.Length == 0)
            {
                return BadRequest("Resim dosyası gereklidir.");
            }

            ImageUploadParams uploadParams = new()
            {
                File = new FileDescription(name: alien.FormFile.FileName, alien.FormFile.OpenReadStream())
            };

            ImageUploadResult uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                return BadRequest($"Resim yükleme hatası: {uploadResult.Error.Message}");
            }

            alien.alienshadow = uploadResult.SecureUrl.AbsoluteUri;
            Business.Utilities.Result.IResult result = _alienService.Add(alien);

            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        [HttpPost("delete")]
        public IActionResult Delete(Alien alien)
        {
            Business.Utilities.Result.IResult result = _alienService.Delete(alien);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpGet("getall")]
        public IActionResult Getall()
        {
            Business.Utilities.Result.IDataResult<List<Alien>> result = _alienService.GetList();
            return result.Success ? Ok(result.Data) : BadRequest(result);
        }
    }

}
