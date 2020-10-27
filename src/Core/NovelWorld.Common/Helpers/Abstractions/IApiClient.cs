using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;

namespace NovelWorld.Common.Helpers.Abstractions
{
    public interface IApiClient
    {
        T Get<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, DataFormat dataFormat = DataFormat.Json);

        Task<T> GetAsync<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, DataFormat dataFormat = DataFormat.Json);

        T Post<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, object body = null, DataFormat dataFormat = DataFormat.Json);

        Task<T> PostAsync<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, object body = null, DataFormat dataFormat = DataFormat.Json);

        T Put<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, object body = null, DataFormat dataFormat = DataFormat.Json);

        Task<T> PutAsync<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, object body = null, DataFormat dataFormat = DataFormat.Json);

        T Patch<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, object body = null, DataFormat dataFormat = DataFormat.Json);

        Task<T> PatchAsync<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, object body = null, DataFormat dataFormat = DataFormat.Json);

        T Delete<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, DataFormat dataFormat = DataFormat.Json);

        Task<T> DeleteAsync<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, DataFormat dataFormat = DataFormat.Json);
    }
}