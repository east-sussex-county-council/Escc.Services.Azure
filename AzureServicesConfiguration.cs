using System;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;

namespace Escc.Services.Azure
{
    /// <summary>
    /// Configuration settings for Azure services
    /// </summary>
    public class AzureServicesConfiguration
    {
        /// <summary>
        /// Gets the storage account used for queues, blobs and tables related to queuing emails.
        /// </summary>
        /// <value>
        /// The storage account.
        /// </value>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">Escc.Services.Azure.EmailQueue connection string for Azure storage not found in app.config</exception>
        public CloudStorageAccount EmailQueueStorageAccount
        {
            get
            {
                if (ConfigurationManager.ConnectionStrings["Escc.Services.Azure.EmailQueue"] == null
                     || String.IsNullOrEmpty(ConfigurationManager.ConnectionStrings["Escc.Services.Azure.EmailQueue"].ConnectionString))
                {
                    throw new ConfigurationErrorsException(
                          "Escc.Services.Azure.EmailQueue connection string for Azure storage not found in app.config");
                }

                return CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["Escc.Services.Azure.EmailQueue"].ConnectionString);
            }
        }
    }
}
