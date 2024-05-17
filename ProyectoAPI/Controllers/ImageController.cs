using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ImageController : ControllerBase
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public ImageController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        [HttpPost("{id:int}")]
        public async Task<IActionResult> UploadImage(int id, string imgBase64)
        {
            //    string path = "D:\\Descargas\\MaterialDesignInXamlToolkit-master\\MaterialDesign3.Demo.Wpf\\Resources\\Contact.png";
            //    byte[] img = System.IO.File.ReadAllBytes(path);
            //    imgBase64 = Convert.ToBase64String(img);

            byte[] imgBytes;
            try
            {
                imgBytes = Convert.FromBase64String(imgBase64);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            if (!IsPng(imgBytes))
            {
                return BadRequest();
            }

            var savePath = Path.Combine(webHostEnvironment.WebRootPath, "Images", id.ToString() + ".png");

            await System.IO.File.WriteAllBytesAsync(savePath, imgBytes);

            return Ok();
        }
        private bool IsPng(byte[] bytes)
        {
            // PNG file signature bytes: 89 50 4E 47 0D 0A 1A 0A
            byte[] pngSignature = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 };
            if (bytes.Length < pngSignature.Length)
            {
                return false;
            }

            for (int i = 0; i < pngSignature.Length; i++)
            {
                if (bytes[i] != pngSignature[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
