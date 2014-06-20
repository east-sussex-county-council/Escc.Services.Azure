
using System;
using System.IO;
using System.Net.Mail;
using System.Runtime.Serialization;
using Microsoft.WindowsAzure.Storage.Blob;
using S22.Mail;

namespace Escc.Services.Azure
{
    /// <summary>
    /// Serialise an email to and from an Azure blob
    /// </summary>
    public class AzureEmailToBlobSerialiser
    {
        private readonly IFormatter _serialisationFormatter;
        private readonly CloudBlobContainer _blobContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureEmailToBlobSerialiser"/> class.
        /// </summary>
        /// <param name="serialisationFormatter">The serialisation formatter.</param>
        public AzureEmailToBlobSerialiser(IFormatter serialisationFormatter)
        {
            _serialisationFormatter = serialisationFormatter;
            _blobContainer = CreateBlobContainer();
        }

        private static CloudBlobContainer CreateBlobContainer()
        {
            var config = new AzureServicesConfiguration();
            var storageAccount = config.EmailQueueStorageAccount;
            var blobClient = storageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference("email");
            blobContainer.CreateIfNotExistsAsync().Wait();
            return blobContainer;
        }

        /// <summary>
        /// Gets the blob container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        public CloudBlobContainer Container { get { return _blobContainer; } }

        /// <summary>
        /// Reads a blob containing an email from its URI.
        /// </summary>
        /// <param name="blobUri">The blob URI.</param>
        /// <returns></returns>
        public CloudBlockBlob ReadBlobFromUri(Uri blobUri)
        {
            var url = blobUri.ToString();
            var guid = url.Substring(url.LastIndexOf("/", StringComparison.Ordinal) + 1);
            return _blobContainer.GetBlockBlobReference(guid);
        }

        /// <summary>
        /// Serialises the specified email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public Uri Serialise(MailMessage email)
        {
            using (var s = new MemoryStream())
            {
                _serialisationFormatter.Serialize(s, (SerializableMailMessage)email);
                s.Seek(0, SeekOrigin.Begin);

                var blob = _blobContainer.GetBlockBlobReference(Guid.NewGuid().ToString());
                blob.UploadFromStream(s);

                return blob.Uri;
            }
        }


        /// <summary>
        /// Deserialises the specified blob.
        /// </summary>
        /// <param name="blob">The blob.</param>
        /// <returns></returns>
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
