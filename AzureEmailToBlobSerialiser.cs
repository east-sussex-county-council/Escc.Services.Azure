
using System;
using System.IO;
using System.Net.Mail;
using System.Runtime.Serialization;
using Microsoft.WindowsAzure.Storage.Blob;
using S22.Mail;

namespace Escc.Services.Azure
{
    public class AzureEmailToBlobSerialiser
    {
        private readonly IFormatter _serialisationFormatter;
        private readonly CloudBlobContainer _blobContainer;

        public AzureEmailToBlobSerialiser(IFormatter serialisationFormatter)
        {
            _serialisationFormatter = serialisationFormatter;
            _blobContainer = CreateBlobContainer();
        }

        private static CloudBlobContainer CreateBlobContainer()
        {
            var config = new AzureServicesConfiguration();
            var storageAccount = config.StorageAccount;
            var blobClient = storageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference("email");
            blobContainer.CreateIfNotExistsAsync().Wait();
            return blobContainer;
        }

        public CloudBlobContainer Container { get { return _blobContainer; } }

        public CloudBlockBlob ReadBlobFromUri(Uri blobUri)
        {
            var url = blobUri.ToString();
            var guid = url.Substring(url.LastIndexOf("/", StringComparison.Ordinal) + 1);
            return _blobContainer.GetBlockBlobReference(guid);
        }

        public Uri Serialise(MailMessage email)
        {
            using (var s = new MemoryStream())
            {
                // Serialize MailMessage to memory stream
                _serialisationFormatter.Serialize(s, (SerializableMailMessage)email);
                s.Seek(0, SeekOrigin.Begin);

                var blob = _blobContainer.GetBlockBlobReference(Guid.NewGuid().ToString());
                blob.UploadFromStream(s);

                return blob.Uri;
            }
        }


        public MailMessage Deserialise(CloudBlockBlob blob)
        {
            using (var s = new MemoryStream())
            {
                blob.DownloadToStream(s);
                s.Seek(0, SeekOrigin.Begin);

                return (SerializableMailMessage)_serialisationFormatter.Deserialize(s);
            }
        }
    }
}
