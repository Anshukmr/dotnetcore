using Auth0.WebApi.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Auth0.WebApi.Helper
{
    public static class AzureStorageHelper
    {

        public static async Task<string> UploadToBlob(string connectionString, string blobContainer, string filename, Guid Id, string docType, Stream stream)
        {
            CloudBlobContainer cloudBlobContainer = null;
            CompanyDbContext context = new CompanyDbContext();//TODO: DI
          
            if (CloudStorageAccount.TryParse(connectionString, out CloudStorageAccount storageAccount))
            {
                try
                {
                    // Create the CloudBlobClient that represents the Blob storage endpoint for the storage account.
                    CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

                    // Create a container called 'uploaddownloadblob' and append a GUID value to it to make the name unique.               
                    cloudBlobContainer = cloudBlobClient.GetContainerReference(blobContainer);

                    await cloudBlobContainer.CreateIfNotExistsAsync();

                    // Get a reference to the blob address, then upload the file to the blob.
                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(Id + "/" + filename);
                    if (stream != null)
                    {

                        cloudBlockBlob.UploadFromStreamAsync(stream, null, null, null).GetAwaiter().GetResult();
                        //Set metadata   
                        //cloudBlockBlob.Metadata["DocType"] = docType;
                        //cloudBlockBlob.SetMetadataAsync().GetAwaiter().GetResult();

                    }
                    else
                    {
                        return "failed";
                    }

                    //SAS Token: Default for Editor.
                    SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy
                    {
                        //Access is Valid for a week
                        SharedAccessExpiryTime = DateTime.UtcNow.AddYears(1),
                        Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write
                    };

                    //Generate the shared access signature on the blob, setting the constraints directly on the signature.
                    string sasBlobToken = cloudBlockBlob.GetSharedAccessSignature(sasConstraints);

                    //Save File location in Document Table:
                    var c = (from r in context.Document where r.Id == Id select r).FirstOrDefault();
                    c.URI = cloudBlockBlob.Uri.AbsoluteUri;
                    c.SASToken = sasBlobToken;
                    c.Title = filename;
                    context.Entry(c).State = EntityState.Modified;
                    context.SaveChanges();

                    return cloudBlockBlob.Uri.AbsoluteUri + sasBlobToken;
                }
                catch (StorageException ex)
                {
                    return "failed";
                }
                finally
                {
                    // OPTIONAL: Clean up resources, e.g. blob container
                    //if (cloudBlobContainer != null)
                    //{
                    //    await cloudBlobContainer.DeleteIfExistsAsync();
                    //}
                }
            }
            else
            {
                return "failed";
            }

        }

        //SAS
        public static string GetBlobSasToken(string connectionString, string blobContainer, string blobName, SharedAccessBlobPermissions permissions, string policyName = null)
        {
            CloudBlobContainer cloudBlobContainer = null;
            string sasBlobToken = "";

            if (CloudStorageAccount.TryParse(connectionString, out CloudStorageAccount storageAccount))
            {
                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

                // Create a container called 'uploaddownloadblob' and append a GUID value to it to make the name unique.               
                cloudBlobContainer = cloudBlobClient.GetContainerReference(blobContainer);

                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(blobName);

                SharedAccessBlobPolicy adHocSas = new SharedAccessBlobPolicy
                {
                    //Access is Valid for a week
                    SharedAccessExpiryTime = DateTime.UtcNow.AddYears(1),
                    Permissions = permissions
                };
                // Generate the shared access signature on the blob, setting the constraints directly on the signature.
                sasBlobToken = cloudBlockBlob.GetSharedAccessSignature(adHocSas, null, null, SharedAccessProtocol.HttpsOrHttp, null);
            }

            return sasBlobToken;

        }
    }
}
