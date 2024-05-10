using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    internal class AzureBlobService : IFileService
    {
        private const string containerName = "images";
        private readonly string connectionString;

        public AzureBlobService(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("AzureBlobStorage")!;
        }
        public async Task DeleteProductImage(string path)
        {
            var client = new BlobContainerClient(connectionString, containerName);

            if (!await client.ExistsAsync()) return;

            string fileName = Path.GetFileName(path);

            var blob = client.GetBlobClient(fileName);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<string> SaveProductImage(IFormFile file)
        {
            var client = new BlobContainerClient(connectionString, containerName);
            await client.CreateIfNotExistsAsync();
            await client.SetAccessPolicyAsync(PublicAccessType.Blob);

            // custom file name
            string name = Guid.NewGuid().ToString();             // random name
            string extension = Path.GetExtension(file.FileName); // get original extension
            string fullName = name + extension;                  // full name: name.ext

            BlobHttpHeaders httpHeaders = new() { ContentType = file.ContentType };

            var blob = client.GetBlobClient(fullName);
            await blob.UploadAsync(file.OpenReadStream(), httpHeaders);

            return blob.Uri.ToString();
        }
    }
}
