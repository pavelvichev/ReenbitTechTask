using AutoFixture;
using Azure.Communication.Email;
using Azure.Storage.Blobs;
using BlobTask;
using BlobTask.Controllers;
using BlobTask.Models;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobFuncTest
{
    public class BlobControllerTest
    {
        BlobSigner _signerUnderTest;
        Mock<BlobContainerClient> _blobContainerClient = new();
        public BlobControllerTest()
        {
            _signerUnderTest = new BlobSigner(_blobContainerClient.Object);
        }

        [Fact]
            public async Task SendToBlob_ValidInput_ReturnsOkResult()
            {
                // Arrange
                var fixture = new Fixture();

                var input = fixture.Create<Mock<Input>>().Object; 

                var fileStream = new MemoryStream();

                fileStream.Write(new byte[4]);

                var formFile = new FormFile(fileStream, 0, fileStream.Length, "file", "file.txt");

                input.File = formFile;

                input.EmailName = "pavelvichev15@gmail.com";

                var blobClientMock = new Mock<BlobClient>();

                var logger = new Mock<ILogger<DocxController>>();

                var settings = new Mock<AppSettings>("connection_string");
              

                var blobServiceClientMock = new Mock<BlobServiceClient>();

                blobServiceClientMock.Setup(client => client.GetBlobContainerClient("blobcontainer"))
                    .Returns(_blobContainerClient.Object);
                blobServiceClientMock.Setup(client => client.GetBlobContainerClient("blobcontainer").GetBlobClient("file.txt"))
                    .Returns(blobClientMock.Object);

                var controller = new Mock<DocxController>(logger.Object, settings.Object, _signerUnderTest);

                // Act
                var result = await controller.Object.SendToBlob(input);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal("File uploaded successfully", okResult.Value);
            }

        [Fact]
            public async Task SendToBlob_InvalidInput_EmptyFile_ReturnsBadResult()
            {
                // Arrange
                var fixture = new Fixture();

                var input = fixture.Create<Mock<Input>>().Object;

                var fileStream = new MemoryStream();


                var formFile = new FormFile(fileStream, 0, fileStream.Length, "file", "file.txt");

                input.File = formFile;

                input.EmailName = "pavelvichev15@gmail.com";

                var blobClientMock = new Mock<BlobClient>();

                var logger = new Mock<ILogger<DocxController>>();

                var settings = new Mock<AppSettings>("connection_string");


                var blobServiceClientMock = new Mock<BlobServiceClient>();

                blobServiceClientMock.Setup(client => client.GetBlobContainerClient("blobcontainer"))
                    .Returns(_blobContainerClient.Object);
                blobServiceClientMock.Setup(client => client.GetBlobContainerClient("blobcontainer").GetBlobClient("file.txt"))
                    .Returns(blobClientMock.Object);

                var controller = new Mock<DocxController>(logger.Object, settings.Object, _signerUnderTest);

                // Act
                var result = await controller.Object.SendToBlob(input);

                // Assert
                var okResult = Assert.IsType<BadRequestObjectResult>(result);
                
                

                Assert.Equal("Empty file",okResult.Value);
            }

        [Fact]
        public async Task SendToBlob_InvalidModel_ReturnsBadResult()
        {
            // Arrange

            var input = new Input();

            var blobClientMock = new Mock<BlobClient>();

            var logger = new Mock<ILogger<DocxController>>();

            var settings = new Mock<AppSettings>("connection_string");


            var blobServiceClientMock = new Mock<BlobServiceClient>();

            blobServiceClientMock.Setup(client => client.GetBlobContainerClient("blobcontainer"))
                .Returns(_blobContainerClient.Object);
            blobServiceClientMock.Setup(client => client.GetBlobContainerClient("blobcontainer").GetBlobClient("file.txt"))
                .Returns(blobClientMock.Object);

            var controller = new Mock<DocxController>(logger.Object, settings.Object, _signerUnderTest);

            controller.Object.ModelState.AddModelError("EmailName", "EmailName is required");
            controller.Object.ModelState.AddModelError("File", "File is required");
            // Act
            var result = await controller.Object.SendToBlob(input);

            // Assert
            var okResult = Assert.IsType<BadRequestObjectResult>(result);

            var errorMessages = okResult.Value as List<string>;
            Assert.NotNull(errorMessages);
            Assert.Equal(2, errorMessages.Count); 


            Assert.Contains("EmailName is required", errorMessages);
            Assert.Contains("File is required", errorMessages);
        }
    }
}
