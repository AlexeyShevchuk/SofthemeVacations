using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Vacations.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        readonly CloudBlobContainer _blobContainer;

        public ImagesController(IConfiguration configuration)
        {
            var storageConnectionString = configuration.GetConnectionString("StorageConnectionString");

            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);

            // We are going to use Blob Storage, so we need a blob client.
            var blobClient = storageAccount.CreateCloudBlobClient();

            // Data in blobs are organized in containers.
            // Here, we create a new, empty container.
            _blobContainer = blobClient.GetContainerReference("images");
            _blobContainer.CreateIfNotExists();

            // We also set the permissions to "Public", so anyone will be able to access the file.
            // By default, containers are created with private permissions only.
            _blobContainer.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
        }

        // GET: api/Images
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Images/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Post()//string imagePath
        {
            try
            {
                using (Stream stream = Response.Body)
                {
                    using (var binaryReader = new BinaryReader(stream))
                    {
                        if (Response.ContentLength != null)
                        {
                            var fileContent = binaryReader.ReadBytes((int)Response.ContentLength);
                            var blockBlob = _blobContainer.GetBlockBlobReference("Guid");

                            await blockBlob.UploadFromByteArrayAsync(fileContent, 0, fileContent.Length);
                        }
                    }
                    // The parameter to the GetBlockBlobReference method will be the name
                    // of the image (the blob) as it appears on the storage server.
                    // You can name it anything you like; in this example, I am just using
                    // the actual filename of the uploaded image.

                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
