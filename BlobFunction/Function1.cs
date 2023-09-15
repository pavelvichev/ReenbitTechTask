using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using Azure.Communication.Email;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using BlobFunction.Configure;
using BlobFunction.ConfigureInterfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BlobFunction
{
    public class Function1
    {
        private readonly ILogger _logger;
        private readonly IBlobStorageSigner _signer;
        private readonly IEmailComposer _emailComposer;
        private readonly AppSettings _enviromentVariables;        
        private  IEmailSender _sender;        
        public Function1(ILoggerFactory loggerFactory, IBlobStorageSigner signer, IEmailComposer emailComposer, AppSettings enviromentVariables, IEmailSender sender)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
            _signer = signer;
            _emailComposer = emailComposer;
            _enviromentVariables = enviromentVariables;
            _sender = sender;
        }

        [Function("Function1")]
        public  async Task Run([BlobTrigger("blobcontainer/{name}", Connection = "BLobConnectionString")] string myBlob, string name, IDictionary<string, string> metadata)
        {
            if (metadata != null)
            {
                if (myBlob == null || name == null || metadata.Count == 0)
                {
                    throw new ArgumentNullException("One of the values can`t be null");
                }

                string connectionString = _enviromentVariables.ConnectionStringEmail;
                string accountName = _enviromentVariables.StorageName;
                string accountKey = _enviromentVariables.AccountKey;
                string containerName = _enviromentVariables.ContainerName;

                string email = "";

                if (metadata.TryGetValue("CustomParameter", out var customParamValue))
                {
                    email = customParamValue.ToString();

                }
                else
                {
                    throw new ArgumentException("Metadata is empty");
                }


                var Uri = await _signer.CreateSASURi(accountName,containerName, accountKey, name);

                var message = _emailComposer.CreateEmail(email, Uri);

                await _sender.Send(message, CancellationToken.None);
            }
            else
            {
                throw new ArgumentNullException("One of the values can`t be null");
            }

           
        }

     
       
    }


}
