using System.Collections.Generic;
using System.Threading.Tasks;
using NovelWorld.Data.Enums;

namespace NovelWorld.Common.Helpers.Abstractions
{
    public interface IApiClient
    {
        T Get<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, ApiBodyTypeEnum bodyType = ApiBodyTypeEnum.Json);

        Task<T> GetAsync<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, ApiBodyTypeEnum bodyType = ApiBodyTypeEnum.Json);

        T Post<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, object body = null, ApiBodyTypeEnum bodyType = ApiBodyTypeEnum.Json);

        Task<T> PostAsync<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, object body = null, ApiBodyTypeEnum bodyType = ApiBodyTypeEnum.Json);

        T Put<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, object body = null, ApiBodyTypeEnum bodyType = ApiBodyTypeEnum.Json);

        Task<T> PutAsync<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, object body = null, ApiBodyTypeEnum bodyType = ApiBodyTypeEnum.Json);

        T Patch<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, object body = null, ApiBodyTypeEnum bodyType = ApiBodyTypeEnum.Json);

        Task<T> PatchAsync<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, object body = null, ApiBodyTypeEnum bodyType = ApiBodyTypeEnum.Json);

        T Delete<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, ApiBodyTypeEnum bodyType = ApiBodyTypeEnum.Json);

        Task<T> DeleteAsync<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, ApiBodyTypeEnum bodyType = ApiBodyTypeEnum.Json);

        T Execute<T>(string url, ApiMethodEnum method, Dictionary<string, object> headers,
            Dictionary<string, object> queries, object body, ApiBodyTypeEnum bodyType = ApiBodyTypeEnum.Json);
        
        string Execute(string url, ApiMethodEnum method, Dictionary<string, object> headers,
            Dictionary<string, object> queries, object body, ApiBodyTypeEnum bodyType = ApiBodyTypeEnum.Json);

        Task<T> ExecuteAsync<T>(string url, ApiMethodEnum method, Dictionary<string, object> headers,
            Dictionary<string, object> queries, object body, ApiBodyTypeEnum bodyType = ApiBodyTypeEnum.Json);
        
        Task<string> ExecuteAsync(string url, ApiMethodEnum method, Dictionary<string, object> headers,
            Dictionary<string, object> queries, object body, ApiBodyTypeEnum bodyType = ApiBodyTypeEnum.Json);
    }
}