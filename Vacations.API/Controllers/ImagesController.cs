using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Vacations.API.Models;
using Vacations.BLL.Models;
using Vacations.BLL.Services;

namespace Vacations.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        CloudBlobContainer _blobContainer;
        IUsersService _usersService;
        IMapper _mapper;

        public ImagesController(IConfiguration configuration, IUsersService usersService, IMapper mapper)
        {
            _usersService = usersService;
            _mapper = mapper;
            try
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
            catch (Exception ex)
            {
                BadRequest(ex.Message);
            }
        }

        // GET: api/Images
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Images/5
        [HttpGet("current")]
        public async Task<string> GetAsync()
        {
            var currentUserEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            var userDto = await _usersService.GetByEmailAsync(currentUserEmail);
            var userModel = _mapper.Map<UserDto, UserModel>(userDto);

            var blockBlob = _blobContainer.GetBlockBlobReference(userModel.EmployeeId.ToString());
            return blockBlob.Uri.AbsoluteUri;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Post()//string imagePath
        {
            try
            {
                var currentUserEmail = User.FindFirst(ClaimTypes.Email)?.Value;

                var userDto = await _usersService.GetByEmailAsync(currentUserEmail);
                var userModel = _mapper.Map<UserDto, UserModel>(userDto);

                var file = Request.Form.Files[0];
                using (var binaryReader = new BinaryReader(file.OpenReadStream()))
                {
                        var fileContent = binaryReader.ReadBytes((int)file.Length);
                        var blockBlob = _blobContainer.GetBlockBlobReference(userModel.EmployeeId.ToString());

                        await blockBlob.UploadFromByteArrayAsync(fileContent, 0, fileContent.Length);
                }
                // The parameter to the GetBlockBlobReference method will be the name
                // of the image (the blob) as it appears on the storage server.
                // You can name it anything you like; in this example, I am just using
                // the actual filename of the uploaded image.

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
