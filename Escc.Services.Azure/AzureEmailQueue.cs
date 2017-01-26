using Microsoft.WindowsAzure.Storage.Queue;
using System.Threading.Tasks;

namespace Escc.Services.Azure
{
    /// <summary>
    /// An Azure queue to store email for later processing
    /// </summary>
    public class AzureEmailQueue
    {
        private readonly CloudQueue _queue;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureEmailQueue"/> class.
        /// </summary>
        public AzureEmailQueue()
        {
            _queue = CreateQueue();
        }

        private static CloudQueue CreateQueue()
        {
            var config = new AzureServicesConfiguration();
            var storageAccount = config.EmailQueueStorageAccount;
            var queueClient = storageAccount.CreateCloudQueueClient();
            
            var queue = queueClient.GetQueueReference("email");

            queue.CreateIfNotExistsAsync().Wait();

            return queue;
        }

        /// <summary>
        /// Gets the email queue.
        /// </summary>
        /// <value>
        /// The queue.
        /// </value>
        public CloudQueue Queue { get { return _queue; } }
    }
}
