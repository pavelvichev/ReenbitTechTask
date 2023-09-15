using Azure.Communication.Email;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlobFunction.ConfigureInterfaces;

namespace BlobFunction.Configure
{
    public class EmailSender : IEmailSender
    {
        EmailClient _client;
        public EmailSender(EmailClient client)
        {
            _client = client;
        }

        public async Task<EmailSendOperation> Send(EmailMessage message, CancellationToken cancellationToken)
        {
            if(message == null)
            {
                throw new ArgumentNullException("Ine of the values can`t be  null");
            }
            return await _client.SendAsync(WaitUntil.Started, message, cancellationToken);
        }
    }
}
