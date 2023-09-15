using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobFunction.Configure
{
    public static class BlobClientExtension
    {
        public static Uri GetReadOnlySASUrl(this BlobClient client, DateTimeOffset expiresAt)
        {
            if (client == null) throw new ArgumentException(nameof(client));

            if (!client.CanGenerateSasUri) throw new InvalidOperationException("SAS URI generation is not supported");

            BlobSasBuilder builder = new()
            {
                BlobContainerName = client.GetParentBlobContainerClient().Name,
                Resource = client.Name,
                ExpiresOn = DateTime.UtcNow.AddHours(5),
            };
            builder.ExpiresOn = DateTime.UtcNow.AddHours(5); builder.SetPermissions(BlobContainerSasPermissions.Read);
            return client.GenerateSasUri(builder);
        }
    }
}
