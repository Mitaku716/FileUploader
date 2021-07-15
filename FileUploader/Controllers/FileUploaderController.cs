using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

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
        [Route("Post")]
        public IActionResult Post([FromForm] IFormFile excelFile,[FromForm] string headerData)
        {
            var convertedHeaderData = JsonConvert.DeserializeObject<List<SomeClass>>(headerData);



            var x = 1;
            return Ok();
        }
    }
    public class SomeClass
    {
        public string key { get; set; }
        public string fieldName { get; set; }
        public bool duplicated { get; set; }
      }
}

