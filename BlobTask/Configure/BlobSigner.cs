using Azure.Storage.Blobs;
using BlobTask.Models;

namespace BlobTask.Configure
{
    public class BlobSigner
    {
        BlobContainerClient client;
        public BlobSigner(BlobContainerClient client)
        {

            this.client = client;

        }


        public async Task<BlobClient> CreateBlob(string fileName)
        {
            var blobContainerClient = client;

            await blobContainerClient.CreateIfNotExistsAsync();

            var blobClient = blobContainerClient.GetBlobClient(fileName);

            return blobClient;
        }
    }
}
