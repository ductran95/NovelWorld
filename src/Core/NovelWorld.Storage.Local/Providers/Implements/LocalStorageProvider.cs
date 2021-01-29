using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NovelWorld.Storage.Configurations;
using NovelWorld.Storage.Exceptions;
using NovelWorld.Storage.Extensions;
using NovelWorld.Storage.Providers.Abstractions;
using NovelWorld.Utility;

namespace NovelWorld.Storage.Local.Providers.Implements
{
    public class LocalStorageProvider: IStorageProvider
    {
        private const string Prefix = "file://";

        private readonly StorageConfiguration _configuration;
        
        public LocalStorageProvider(IOptions<StorageConfiguration> configuration)
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

            var filePath = path.Replace(Prefix, string.Empty);

            if (!File.Exists(filePath))
            {
                throw new StorageItemNotFoundException(path);
            }

            var fileName = Path.GetFileName(filePath);
            var memoryStream = new MemoryStream();
            await using (var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                await fileStream.CopyToAsync(memoryStream, cancellationToken);
            }

            return (memoryStream, fileName);
        }

        public async Task<string> Create(Stream stream, string name, CancellationToken cancellationToken = default)
        {
            Check.NotNullOrEmpty(stream);
            Check.NotNullOrEmpty(name);
            
            var filePath = Path.Combine(_configuration.RootPath, name);
            
            if (File.Exists(filePath))
            {
                throw new StorageItemExistedException(filePath);
            }
            
            await using (var fileStream = File.Open(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                await stream.CopyToAsync(fileStream, cancellationToken);
            }

            return $"{Prefix}{filePath}";
        }

        public async Task<string> Update(string path, Stream stream, string name, CancellationToken cancellationToken = default)
        {
            Check.NotNullOrEmpty(path);
            Check.NotNullOrEmpty(stream);
            Check.NotNullOrEmpty(name);
            
            var filePath = path.Replace(Prefix, string.Empty);

            if (!File.Exists(filePath))
            {
                throw new StorageItemNotFoundException(path);
            }
            
            await using (var fileStream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                await stream.CopyToAsync(fileStream, cancellationToken);
            }
            
            filePath = Path.Combine(_configuration.RootPath, name);

            return $"{Prefix}{filePath}";
        }

        public Task Delete(string path, CancellationToken cancellationToken = default)
        {
            Check.NotNullOrEmpty(path);
            
            var filePath = path.Replace(Prefix, string.Empty);

            if (!File.Exists(filePath))
            {
                throw new StorageItemNotFoundException(path);
            }
            
            File.Delete(filePath);
            
            return Task.CompletedTask;
        }
    }
}