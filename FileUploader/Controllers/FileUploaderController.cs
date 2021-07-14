using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace FileUploader.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileUploaderController : ControllerBase
    {
        private readonly ILogger<FileUploaderController> _logger;

        public FileUploaderController(ILogger<FileUploaderController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[]{"Date Of Birth", "Original Account Number"};
        }
    }
}
