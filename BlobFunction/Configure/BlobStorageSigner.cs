using Azure.Storage.Blobs;
using Azure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using BlobFunction.ConfigureInterfaces;
using Azure;

namespace BlobFunction.Configure
{
    public class BlobStorageSigner : IBlobStorageSigner
    {
        public string Url { get; private set; }

        public async Task<string> CreateSASURi(string accountName, string blobContainer,string accountKey, string name)
        {

                if(accountName == null || blobContainer == null || accountKey == null || name == null)
                {
                    throw new ArgumentNullException("One of the values can`t be null");
                }

                StorageSharedKeyCredential storageSharedKeyCredential =
                  new(accountName, accountKey);
                BlobClient blobClient = new BlobClient(
                    new Uri($"https://{accountName}.blob.core.windows.net/{blobContainer}/{name}"),
                    storageSharedKeyCredential);

                var urlBlob = blobClient.GetReadOnlySASUrl(expiresAt: DateTime.UtcNow.AddHours(1)).ToString();


                Url = urlBlob;
                return urlBlob;


        }

        
    }
}
