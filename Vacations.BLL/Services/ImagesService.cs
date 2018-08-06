using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Vacations.BLL.Services
{
    public class ImagesService : IImagesService
    {
        private CloudBlobContainer _blobContainer;
        private readonly CloudBlobClient _blobClient;

        public ImagesService(
            IConfiguration configuration
            )
        {
            var storageConnectionString = configuration.GetConnectionString("StorageConnectionString");

            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);

            _blobClient = storageAccount.CreateCloudBlobClient();

            _blobContainer = _blobClient.GetContainerReference("images");

            _blobContainer.CreateIfNotExistsAsync();

            _blobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

        }

        public async Task<string> GetUrlAsync(string imgName)
        {
            CloudBlockBlob blockBlob;

            if (imgName == null)
            {
                blockBlob = _blobContainer.GetBlockBlobReference("default");
                return blockBlob.Uri.AbsoluteUri;
            }

            blockBlob = _blobContainer.GetBlockBlobReference(imgName);

            if (await blockBlob.ExistsAsync())
            {
                return blockBlob.Uri.AbsoluteUri;
            }

            blockBlob = _blobContainer.GetBlockBlobReference("default");

            return blockBlob.Uri.AbsoluteUri;
        }

        public async Task UploadAsync(IFormFile file)
        {
            using (var binaryReader = new BinaryReader(file.OpenReadStream()))
            {
                var fileContent = binaryReader.ReadBytes((int)file.Length);

                var blockBlob = _blobContainer.GetBlockBlobReference(file.Name);

                await blockBlob.UploadFromByteArrayAsync(fileContent, 0, fileContent.Length);
            }
        }
    }
}
