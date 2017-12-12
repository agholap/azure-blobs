using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Blobs
{
    class Program
    {
        static void Main(string[] args)
        {
            string storageConnection = System.Configuration.ConfigurationManager.AppSettings.Get("storgeAccountKey");
            CloudStorageAccount stoAccount = CloudStorageAccount.Parse(storageConnection);

            CloudBlobClient blobClient = stoAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("objective2");

            container.CreateIfNotExists();

            CloudBlockBlob blockBlob = container.GetBlockBlobReference("examobjectives");
            using (var fileStram = System.IO.File.OpenRead(@"C:\Users\amol.gholap\Downloads\532_OD_Changes.pdf"))
            {
                blockBlob.UploadFromStream(fileStram);
            }
            //ListAttributes(container);

            // CopyBlob(container);
            UploadBlobSubDirectory(container);

        }

        static void ListAttributes(CloudBlobContainer container)
        {
            container.FetchAttributes();
            Console.WriteLine("Container Name " + container.StorageUri.PrimaryUri.ToString());
            Console.WriteLine("Last Modified " + container.Properties.LastModified.ToString());
            SetMetadata(container);
            ListMetadata(container);
        }
        static void ListMetadata(CloudBlobContainer container)
        {
            container.FetchAttributes();
            Console.WriteLine("Metadta:\n");
            foreach (var item in container.Metadata)
            {
                Console.WriteLine("Key " + item.Key);
                Console.WriteLine("Value " + item.Value);
            }

        }
        static void SetMetadata(CloudBlobContainer container)
        {
            container.Metadata.Clear();
            container.Metadata.Add("author", "Amol Gholap");
            container.Metadata["authoredOn"] = "Dec 12, 2017";
            container.SetMetadata();
        }

        static void CopyBlob(CloudBlobContainer container)
        {
            CloudBlockBlob blockBlob = container.GetBlockBlobReference("examobjectives");
            CloudBlockBlob copyToBlockBlob = container.GetBlockBlobReference("examobjectives-copy");
            copyToBlockBlob.StartCopyAsync(new Uri(blockBlob.Uri.AbsoluteUri));
        }

        static void UploadBlobSubDirectory(CloudBlobContainer container)
        {
            CloudBlobDirectory directory = container.GetDirectoryReference("parent-directory");
            CloudBlobDirectory subdirectory = directory.GetDirectoryReference("child-directory");
            CloudBlockBlob blockBlob = subdirectory.GetBlockBlobReference("newexamobjectives");

            using (var fileStream = System.IO.File.OpenRead(@"C:\Users\amol.gholap\Downloads\532_OD_Changes.pdf"))
            {
                blockBlob.UploadFromStream(fileStream);
            }
        }
    }
}
