using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using DotNetEnv;

namespace RazorWebPageCloud.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            Blobs = CreateClient().GetBlobs().ToList();
        }

        bool isDeployment = true;

        private string getConnectionString()
        {
            if(!isDeployment)
            {
                // read the .env file
                Env.Load();
            }
            
            // read the env vars directly from Azure
            return Environment.GetEnvironmentVariable("BLOB_STORAGE");
        }

        private string getContainerName()
        {
            if(!isDeployment)
            {
                Env.Load();
            }

            return Environment.GetEnvironmentVariable("BLOB_CONTAINER");
        }

        public List<BlobItem>? Blobs { get; set; }

        public FileStreamResult OnGetDownloadBlob(string blobName)
        {
            var blobClient = CreateClient().GetBlobClient(blobName);
            var contentType = blobClient.GetProperties().Value.ContentType;
            var stream = blobClient.OpenRead(); // není tøeba øešit Dispose - stream nám uzavøe infrastruktura Razor Pages.
            return new FileStreamResult(stream, contentType);
        }

        private BlobContainerClient CreateClient()
        {
            string connectionString = getConnectionString();
            Console.WriteLine(connectionString);
            return new Azure.Storage.Blobs.BlobContainerClient(getConnectionString(), getContainerName());
        }
    }
}
