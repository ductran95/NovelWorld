using System.IO;
using System.Threading.Tasks;

namespace NovelWorld.Storage.Providers.Abstractions
{
    public interface IStorageProvider
    {
        Task<(Stream Stream, string Name)> Get(string path);
        Task<string> Create(Stream stream, string name);
        Task<string> Update(string path, Stream stream, string name);
        Task Delete(string path);
    }
}