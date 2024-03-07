using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

        // normally we would add this to the Azure cfg...
        private const string AzureStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=............;EndpointSuffix=core.windows.net";
        private const string AzureStorageContainerName = "...";

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
            return new Azure.Storage.Blobs.BlobContainerClient(AzureStorageConnectionString, AzureStorageContainerName);
        }
    }
}
