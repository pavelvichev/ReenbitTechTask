using AutoFixture;
using AutoFixture.AutoMoq;
using Azure;
using Azure.Communication.Email;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using BlobFunction;
using BlobFunction.Configure;
using BlobFunction.ConfigureInterfaces;
using Castle.Core.Smtp;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.ComponentModel.Design;
using System.Net.Mail;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;

namespace BlobFuncTest
{
    public class BlobFuncTests
    {
        EmailSender _systemUnderTest;
        Mock<EmailClient> _emailClient = new();
        
        public BlobFuncTests()
        {

            _systemUnderTest = new EmailSender(_emailClient.Object);
        }

        [Fact]
        public void CreateEmail_ValidInput_ReturnsEmailMessage()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());

            var emailComposer = fixture.Create<Mock<EmailComposer>>().Object;

            var email = "test@example.com";
            var uri = "https://example.com/file.txt";

            // Act
            var result = emailComposer.CreateEmail(email, uri);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("DoNotReply@91e0f0c2-7392-477f-8e85-3b7a03bb7a6a.azurecomm.net", result.SenderAddress);
            Assert.Equal(email, result.Recipients.To[0].Address);
            Assert.Contains(uri, result.Content.PlainText);

        }

        [Fact]
        public void CreateEmail_InvalidInput_ThrowError()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());

            // Create an instance of the EmailComposer class
            var emailComposer = fixture.Create<Mock<EmailComposer>>().Object;

            // Generate test data using AutoFixture
            var email = "test@example.com";
            var uri = "https://example.com/file.txt";

            //Act Assert

            emailComposer.Should().NotBeNull();
            emailComposer.Invoking(y => y.CreateEmail(null, null)).Should().Throw<ArgumentNullException>();

        }

        [Fact]
        public async Task CreateSASURi_ValidInput_ReturnsSASUri()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());

            var accountName = fixture.Create<string>();
            var accountKey = fixture.Create<string>();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(accountKey);
            accountKey = Convert.ToBase64String(bytes);
            var containerName = "container";
            var blobName = fixture.Create<string>();


            var fakeSasUri = new Uri($"https://{accountName}.blob.core.windows.net/{containerName}/{blobName}?");

            var signer = fixture.Create<Mock<BlobStorageSigner>>().Object;

            // Act
            var result = await signer.CreateSASURi(accountName, containerName, accountKey, blobName);

            // Assert
            Assert.True(result.Contains(fakeSasUri.ToString()));
        }

        [Fact]
        public async Task CreateSASURi_InvalidInput_ReturnsError()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());

            var accountName = fixture.Create<string>();
            var accountKey = fixture.Create<string>();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(accountKey);
            accountKey = Convert.ToBase64String(bytes);
            var containerName = "container";
            var blobName = fixture.Create<string>();


            var fakeSasUri = new Uri($"https://{accountName}.blob.core.windows.net/{containerName}/{blobName}?");

            var signer = fixture.Create<Mock<BlobStorageSigner>>().Object;
            
            //Act Assert
            signer.Invoking(x=>x.CreateSASURi(null,null, null, null)).Should().ThrowAsync<ArgumentNullException>();
            
        }


        [Fact]
        public async Task Run_ShouldSendEmail()
        {

            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());

            // Arrange

            var emailComposerMock = fixture.Create<Mock<EmailComposer>>();
            var blobStorageSignerMock = fixture.Create<Mock<BlobStorageSigner>>();
            var loggerFactoryMock = fixture.Create<Mock<ILoggerFactory>>();
            
            var envVariables = new AppSettings("hello", "storage_name", "ZGFkW2FzbGRhc3NkYV1zZGE=", "container_name");

            var email = "hello@example.com";

            var myFuncMock = new Mock<Function1>(loggerFactoryMock.Object,
                                                blobStorageSignerMock.Object,
                                                emailComposerMock.Object,
                                                envVariables,
                                                _systemUnderTest);

            var metadata = new Dictionary<string, string>
            {
                { "CustomParametr", email }
            };

            // Act

            WaitUntil? waitUntillArgument = null;
            EmailMessage messageArgument = null;

            _emailClient.Setup(x => x.SendAsync(It.IsAny<WaitUntil>(), It.IsAny<EmailMessage>(), default))
            .Callback((WaitUntil waitUntill, EmailMessage message, CancellationToken _) => {
                waitUntillArgument = waitUntill;
                messageArgument = message;
            });

            await myFuncMock.Object.Run("myBlobData", "blobName", metadata);

            var emailMessage = new EmailMessage("DoNotReply@91e0f0c2-7392-477f-8e85-3b7a03bb7a6a.azurecomm.net", new EmailRecipients(new List<EmailAddress> { new EmailAddress(email) }), new EmailContent("Adding Success") { PlainText = @$"The file has been uploaded successfully. You can access it using the following link: {blobStorageSignerMock.Object.Url}" });

            //Assert

            waitUntillArgument.Should().NotBeNull().And.Be(WaitUntil.Started);
            messageArgument.Should().NotBeNull().And.BeEquivalentTo(emailMessage);

        }

        [Fact]
        public async Task Run_ShouldThrowError()
        {

            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());

            // Arrange

            var emailComposerMock = fixture.Create<Mock<EmailComposer>>();
            var blobStorageSignerMock = fixture.Create<Mock<BlobStorageSigner>>();
            var loggerFactoryMock = fixture.Create<Mock<ILoggerFactory>>();

            var envVariables = new AppSettings("hello", "storage_name", "ZGFkW2FzbGRhc3NkYV1zZGE=", "container_name");

            var email = "";

            var myFuncMock = new Mock<Function1>(loggerFactoryMock.Object,
                                                blobStorageSignerMock.Object,
                                                emailComposerMock.Object,
                                                envVariables,
                                                _systemUnderTest);

            var metadata = new Dictionary<string, string>
            {
                { "CustomParameter", email }
            };

            // Act

            WaitUntil? waitUntillArgument = null;
            EmailMessage messageArgument = null;

            _emailClient.Setup(x => x.SendAsync(It.IsAny<WaitUntil>(), It.IsAny<EmailMessage>(), default))
            .Callback((WaitUntil waitUntill, EmailMessage message, CancellationToken _) => {
                waitUntillArgument = waitUntill;
                messageArgument = message;
            });

            //Assert

           myFuncMock.Object.Invoking(x => x.Run(null, null, null)).Should().ThrowAsync<ArgumentNullException>();

        }



    }
}