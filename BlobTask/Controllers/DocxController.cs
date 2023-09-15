using Azure.Storage.Blobs;
using BlobTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace BlobTask.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class DocxController : ControllerBase
    {

        ILogger<DocxController> _logger;
        AppSettings settings;
        BlobSigner _blobSigner;
           
        public DocxController(ILogger<DocxController> logger, AppSettings settings, BlobSigner blobSigner) 
        {
            _logger = logger;
            this.settings = settings;
            _blobSigner = blobSigner;
        }

        [HttpGet]
        public IActionResult Home()
        {
            return Ok("hi");
        }


        [HttpPost]
        public async Task<IActionResult> SendToBlob([FromForm] Input input)
        {
           
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                return new BadRequestObjectResult(errors);
            }

            if (input.File.Length == 0)
            {
                var emptyFile = "Empty file";
                return new BadRequestObjectResult(emptyFile);
            }


            try
            {
                var blobClient = await _blobSigner.CreateBlob(input.File.FileName);

                using (Stream stream = input.File.OpenReadStream())
                {
                    IDictionary<string, string> metadata = new Dictionary<string, string>();
                    metadata["CustomParameter"] = input.EmailName;

                    await blobClient.UploadAsync(stream, true);

                    blobClient.SetMetadata(metadata);
                }

                return new OkObjectResult("File uploaded successfully");
            }
            catch (Exception ex)
            {
              
                return new BadRequestObjectResult($"An error occurred: {ex.Message}");
            }
        }



    }
}
