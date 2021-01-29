using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using NovelWorld.Storage.Configurations;
using NovelWorld.Storage.Exceptions;
using NovelWorld.Storage.Providers.Abstractions;
using NovelWorld.Utility;

namespace NovelWorld.Storage.AzureBlob.Providers.Implements
{
    public class AzureBlobStorageProvider: IStorageProvider
    {
        private const string Prefix = "azure://";

        private readonly StorageConfiguration _configuration;
        
        private BlobContainerClient _containerClient;
        
        public AzureBlobStorageProvider(IOptions<StorageConfiguration> configuration)
        {
            _configuration = configuration.Value;
        }

        public async Task<(Stream Stream, string Name)> Get(string path, CancellationToken cancellationToken = default)
        {
            Check.NotNullOrEmpty(path);
            
            if (!path.StartsWith(Prefix))
            {
                throw new WrongStorageItemPathException(path);
            }
            
            var containerClient = await GetBlobContainerClient(cancellationToken);
            string filePath = path.Replace(Prefix, string.Empty);
            var fileName = Path.GetFileName(filePath);
            BlobClient blobClient = containerClient.GetBlobClient(filePath);

            var isExist = await blobClient.ExistsAsync(cancellationToken);
            
            if (!isExist)
            {
                throw new StorageItemNotFoundException(path);
            }

            var response = await blobClient.DownloadAsync(cancellationToken);

            if (response.GetRawResponse().Status == 200)
            {
                var stream = new MemoryStream();
                await response.Value.Content.CopyToAsync(stream, cancellationToken);
                return (stream, fileName);
            }
            
            throw new GetStorageErrorException(path, response.GetRawResponse().ReasonPhrase);
        }

        public async Task<string> Create(Stream stream, string name, CancellationToken cancellationToken = default)
        {
            Check.NotNullOrEmpty(stream);
            Check.NotNullOrEmpty(name);
            
            var containerClient = await GetBlobContainerClient(cancellationToken);

            BlobClient blobClient = containerClient.GetBlobClient(name);

            var response = await blobClient.UploadAsync(stream, false, cancellationToken);
            
            if (response.GetRawResponse().Status == 201)
            {
                return $"{Prefix}{name}";
            }

            throw new SaveStorageErrorException(name, response.GetRawResponse().ReasonPhrase);
        }

        public async Task<string> Update(string path, Stream stream, string name, CancellationToken cancellationToken = default)
        {
            Check.NotNullOrEmpty(path);
            Check.NotNullOrEmpty(stream);
            Check.NotNullOrEmpty(name);
            
            if (!path.StartsWith(Prefix))
            {
                throw new WrongStorageItemPathException(path);
            }
            
            var containerClient = await GetBlobContainerClient(cancellationToken);
            string filePath = path.Replace(Prefix, string.Empty);
            BlobClient blobClient = containerClient.GetBlobClient(filePath);

            var isExist = await blobClient.ExistsAsync(cancellationToken);
            
            if (!isExist)
            {
                throw new StorageItemNotFoundException(path);
            }
            
            if (name != filePath)
            {
                BlobClient blobClientNew = containerClient.GetBlobClient(name);
                var response = await blobClientNew.UploadAsync(stream, false, cancellationToken);
                
                if (response.GetRawResponse().Status == 201)
                {
                    await blobClient.DeleteAsync(cancellationToken: cancellationToken);

                    return $"{Prefix}{name}";
                }
                throw new SaveStorageErrorException(name, response.GetRawResponse().ReasonPhrase);
            }
            else
            {
                var response = await blobClient.UploadAsync(stream, true, cancellationToken);
                if (response.GetRawResponse().Status == 201)
                {
                    return $"{Prefix}{name}";
                }

                throw new SaveStorageErrorException(name, response.GetRawResponse().ReasonPhrase);
            }
        }

        public async Task Delete(string path, CancellationToken cancellationToken = default)
        {
            Check.NotNullOrEmpty(path);
            
            if (!path.StartsWith(Prefix))
            {
                throw new WrongStorageItemPathException(path);
            }
            
            var containerClient = await GetBlobContainerClient(cancellationToken);
            string filePath = path.Replace(Prefix, string.Empty);
            BlobClient blobClient = containerClient.GetBlobClient(filePath);

            var isExist = await blobClient.ExistsAsync(cancellationToken);
            
            if (!isExist)
            {
                throw new StorageItemNotFoundException(path);
            }
            
            var response = await blobClient.DeleteAsync(cancellationToken: cancellationToken);
            
            if (response.Status == 200)
            {
                return;
            }

            throw new SaveStorageErrorException(path, response.ReasonPhrase);
        }

        private async Task<BlobContainerClient> GetBlobContainerClient(CancellationToken cancellationToken = default)
        {
            if (_containerClient == null)
            {
                // Create a BlobServiceClient object which will be used to create a container client
                BlobServiceClient blobServiceClient = new BlobServiceClient(_configuration.ConnectionString);

                // Create the container and return a container client object
                _containerClient = await blobServiceClient.CreateBlobContainerAsync(_configuration.RootPath, cancellationToken: cancellationToken);
            }

            return _containerClient;
        }
    }
}