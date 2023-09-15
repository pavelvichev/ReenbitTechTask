using Azure.Storage.Blobs;
using BlobTask;
using BlobTask.Configure;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

Environment.SetEnvironmentVariable("CONFIGURE_STRING_STORAGE", builder.Configuration.GetSection("CustomConfiguration").GetSection("CONFIGURE_STRING_STORAGE").Value);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["BlobKey:blob"], preferMsi: true);
    clientBuilder.AddQueueServiceClient(builder.Configuration["BlobKey:queue"], preferMsi: true);
    
});


var appSettings = new AppSettings
(
    Environment.GetEnvironmentVariable("CONFIGURE_STRING_STORAGE")
);

var blobService = new BlobServiceClient(appSettings.CONFIGURE_STRING_STORAGE);

var blobContainerService = blobService.GetBlobContainerClient("blobcontainer");

builder.Services.AddSingleton(appSettings);

builder.Services.AddSingleton<BlobSigner>(op => new BlobSigner(blobContainerService));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
