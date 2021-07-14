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
        public IEnumerable<FieldName> Get()
        {
            return new FieldName[]{
                new FieldName{
                    Name ="Date Of Birth",
                    PropertyType = "DateTime"
                },
                new FieldName{ 
                    Name = "Original Account Number",
                    PropertyType = "String"
                },
                new FieldName{ 
                    Name = "Original Balance",
                    PropertyType = "decimal"
                }
            };
        }

        [HttpPost]
        public IActionResult Post([FromForm] IFormFile excelFile)
        {
           
        }
    }
}
