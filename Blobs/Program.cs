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
            using (var fileStram = System.IO.File.OpenRead(@"Downloads\532_OD_Changes.pdf"))
            {
                blockBlob.UploadFromStream(fileStram);
            }
        }
    }
}
