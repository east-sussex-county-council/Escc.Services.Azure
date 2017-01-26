using System;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Queue;
using Modules.JsonNet;

namespace Escc.Services.Azure
{
    /// <summary>
    /// Sends emails to an Azure queue for later processing
    /// </summary>
    public class AzureQueuedEmailSender : IEmailSender
    {
        private readonly CloudQueue _queue;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureQueuedEmailSender"/> class.
        /// </summary>
        public AzureQueuedEmailSender()
        {
            _queue = new AzureEmailQueue().Queue;
        }

        /// <summary>
        /// Adds an email to the queue.
        /// </summary>
        public void Send(MailMessage message)
        {
            throw new NotImplementedException("AzureQueuedEmailSender only supports SendAsync");
        }

        /// <summary>
        /// Adds an email to the queue.
        /// </summary>
        /// <param name="message">The email.</param>
        public async Task SendAsync(MailMessage message)
        {
            var blobSerialiser = new AzureEmailToBlobSerialiser(new JsonNetFormatter());
            var blobUri = blobSerialiser.Serialise(message);
            var queueMessage = new CloudQueueMessage(blobUri.ToString());
            
            await _queue.AddMessageAsync(queueMessage);        
        }
    }
}
