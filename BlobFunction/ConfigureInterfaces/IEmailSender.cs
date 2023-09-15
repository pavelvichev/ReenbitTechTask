using Azure.Communication.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobFunction.ConfigureInterfaces
{
    public interface IEmailSender
    {
        Task<EmailSendOperation> Send(EmailMessage message, CancellationToken cancellationToken);
    }
}
