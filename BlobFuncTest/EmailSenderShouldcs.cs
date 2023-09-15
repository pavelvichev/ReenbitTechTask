using Azure.Communication.Email;
using Azure;
using Moq;
using BlobFunction.Configure;
using FluentAssertions;

namespace BlobFuncTest
{
    public class EmailSenderShould
    {
        EmailSender _systemUnderTest;
        Mock<EmailClient> _emailClient = new();
        public EmailSenderShould()
        {

            _systemUnderTest = new EmailSender(_emailClient.Object);
        }

        [Fact]
        public async Task SendEmail()
        {
            WaitUntil? waitUntillArgument = null;
            EmailMessage messageArgument = null;

            _emailClient.Setup(x => x.SendAsync(It.IsAny<WaitUntil>(), It.IsAny<EmailMessage>(), default))
            .Callback((WaitUntil waitUntill, EmailMessage message, CancellationToken _) =>
            {
                waitUntillArgument = waitUntill;
                messageArgument = message;
            });

            var emailMessage = new EmailMessage("sender_abc@localhost.com", "aaa@localhost.com", new EmailContent("lorem ipsum dolor sit amet"));

            await _systemUnderTest.Send(emailMessage, default);
 
            waitUntillArgument.Should().NotBeNull().And.Be(WaitUntil.Started);
            messageArgument.Should().NotBeNull().And.BeEquivalentTo(emailMessage);


        }


        [Fact]
        public async Task SendEmail_Invalid_ReturnsError()
        {
            WaitUntil? waitUntillArgument = null;
            EmailMessage messageArgument = null;

            _emailClient.Setup(x => x.SendAsync(It.IsAny<WaitUntil>(), It.IsAny<EmailMessage>(), default))
            .Callback((WaitUntil waitUntill, EmailMessage message, CancellationToken _) =>
            {
                waitUntillArgument = waitUntill;
                messageArgument = message;
            });


           await _systemUnderTest.Invoking(x => x.Send(null, default)).Should().ThrowAsync<ArgumentNullException>();


        }
    }
}
