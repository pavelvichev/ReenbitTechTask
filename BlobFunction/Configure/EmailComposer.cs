using Azure.Communication.Email;
using BlobFunction.ConfigureInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobFunction.Configure
{
    public class EmailComposer : IEmailComposer
    {

        public EmailMessage CreateEmail(string email, string uri)
        {

                if (email == null || uri == null)
                {
                    throw new ArgumentNullException("One of the values can`t be null");
                }

                EmailContent emailContent = new EmailContent("Adding Success");
                emailContent.PlainText = @$"The file has been uploaded successfully. You can access it using the following link: {uri}";


                List<EmailAddress> addresses = new List<EmailAddress> { new EmailAddress(email) };

                EmailRecipients emailRecipients = new EmailRecipients(addresses);

                EmailMessage message = new EmailMessage("DoNotReply@91e0f0c2-7392-477f-8e85-3b7a03bb7a6a.azurecomm.net", emailRecipients, emailContent);

                return message;
           }
        }
    }
