using Azure.Communication.Email;
using BlobFunction;
using BlobFunction.Configure;
using BlobFunction.ConfigureInterfaces;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var appSettings =
new AppSettings
(
    Environment.GetEnvironmentVariable("ConnectionStringEmail"),
   Environment.GetEnvironmentVariable("StorageName"),
    Environment.GetEnvironmentVariable("AccountKey"),
    Environment.GetEnvironmentVariable("ContainerName")
);

var client = new EmailClient(appSettings.ConnectionStringEmail);
var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(builder =>
    {
        builder.Services.AddSingleton<IBlobStorageSigner,BlobStorageSigner>();
        builder.Services.AddSingleton<IEmailComposer,EmailComposer>();
        builder.Services.AddSingleton(appSettings);
        builder.Services.AddSingleton<IEmailSender,EmailSender>(op => new EmailSender(client));
    })
    .Build();


host.Run();
