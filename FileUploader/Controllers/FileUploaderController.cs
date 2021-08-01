using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.IO;
using ExcelDataReader;
using FileUploader.Models;

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
            //You can go to your business layer or just return any object's property names that way you can provide the user a list of options
            //they can map their columns to.
            var result = new List<FieldName>();
            foreach (var property in typeof(CustomerDataPoint).GetProperties())
            {
                result.Add(new FieldName
                {
                    Name = property.Name,
                    PropertyType = property.PropertyType.Name
                });
            }
            return result;
        }

        [HttpPost]
        [Route("Post")]
        public IActionResult Post([FromForm] IFormFile excelFile, [FromForm] string headerData)
        {
            var convertedHeaderData = JsonConvert.DeserializeObject<List<ExcelColumnMap>>(headerData);

            //If they haven't passed along what fields we need to map the data to then we cant proceed.
            if (convertedHeaderData == null)
            {
                return BadRequest("No Header Data Provided.");
            }

            //Byte size 0 means something is most likely correct with the file.
            if (excelFile.Length == 0)
            {
                return BadRequest("Please check your excel file.");
            }

            List<CustomerDataPoint> compiledDataPoints = new List<CustomerDataPoint>();
            
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = new MemoryStream())
            {
                excelFile.CopyTo(stream);
                stream.Position = 0;

                using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                {
                    //Here we convert this to a datatable so that way we are able to use the header names as our indexer
                    var dataTable = reader.AsDataSet(new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = (_)=> new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true
                        }
                    }).Tables[0];

                    foreach (DataRow dataTableRow in dataTable.Rows)
                    {
                        compiledDataPoints.Add(CreateAndHydrateObject<CustomerDataPoint>(convertedHeaderData, dataTableRow));
                    }

                }
            }

            //At this point you can send this up to the data layer or work with the data itself that has been sent over and converted to your object.

            return Ok();
        }

        /// <summary>
        /// Creates a specified object with the type T and hydrates the data using Excel Row Data.
        /// </summary>
        /// <typeparam name="T">The class the method will return.</typeparam>
        /// <param name="convertedHeaderData">The mapping for where data should be stored in the object</param>
        /// <param name="rowData">The data for a given row in an excel spreadsheet</param>
        /// <returns>Returns a hydrated object of the given class with provided data from the row.</returns>
        private static T CreateAndHydrateObject<T>(List<ExcelColumnMap> convertedHeaderData, DataRow rowData) where T : new()
        {
            var rowObject = new T();

            foreach (var headerStuff in convertedHeaderData)
            {
                var propertyToSet = rowObject.GetType().GetProperty(headerStuff.FieldName);
                propertyToSet?.SetValue(rowObject, Convert.ChangeType(rowData[headerStuff.Key], propertyToSet.PropertyType));
            }

            return rowObject;
        }
    }
}

