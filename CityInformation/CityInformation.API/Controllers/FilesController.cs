using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInformation.API.Controllers
{
    [ApiController(), Route("api/files")]
    public class FilesController : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider _ctp;
        public FilesController(FileExtensionContentTypeProvider ctp) => _ctp = ctp ?? throw new ArgumentNullException(nameof(ctp));

        [HttpGet("{id}")]
        public ActionResult GetFile(string id)
        {
            var path = "download.txt";

            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }

            if (!_ctp.TryGetContentType(path, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, contentType, Path.GetFileName(path));
        }
    }
}
