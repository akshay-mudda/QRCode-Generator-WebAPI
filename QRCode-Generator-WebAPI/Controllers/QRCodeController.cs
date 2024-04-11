using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System;
using System.Drawing;
using System.IO;

namespace QRCode_Generator_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QRCodeController : ControllerBase
    {
        [HttpGet]
        public IActionResult GenerateOrDisplayQRCode([FromQuery] string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return BadRequest("Data cannot be empty");
            }

            using var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(20);

            // Convert the image to byte array
            MemoryStream ms = new MemoryStream();
            qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            byte[] byteImage = ms.ToArray();

            // Return the image as inline content
            return File(byteImage, "image/png");
        }
    }
}
