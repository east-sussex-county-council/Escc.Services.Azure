using System;
using System.Collections.Specialized;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;

namespace Escc.Services.Azure
{
    public class AzureServicesConfiguration
    {
        public CloudStorageAccount StorageAccount
        {
            get
            {
                var config = ConfigurationManager.GetSection("Escc.Services.Azure/EmailQueue") as NameValueCollection;
                if (config == null || String.IsNullOrEmpty(config["StorageAccount"]))
                {
                    throw new ConfigurationErrorsException(
                          "StorageAccount configuration setting not found in Escc.Services.Azure/EmailQueue");
                }

                return CloudStorageAccount.Parse(config["StorageAccount"]);
            }
        }
    }
}
