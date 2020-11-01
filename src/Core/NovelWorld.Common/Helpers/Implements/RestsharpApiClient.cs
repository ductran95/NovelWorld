using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NovelWorld.Common.Exceptions;
using NovelWorld.Common.Extensions;
using NovelWorld.Common.Helpers.Abstractions;
using NovelWorld.Data.Enums;
using RestSharp;

namespace NovelWorld.Common.Helpers.Implements
{
    public class RestsharpApiClient : IApiClient
    {
        private readonly IRestClient _client;
        private readonly ILogger<RestsharpApiClient> _logger;

        public RestsharpApiClient(ILogger<RestsharpApiClient> logger, IRestClient client)
        {
            _logger = logger;
            _client = client;
        }

        public T Get<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, ApiBodyTypeEnum bodyType = ApiBodyTypeEnum.Json)
        {
            return Execute<T>(url, ApiMethodEnum.GET, headers, queries, null, bodyType);
        }

        public async Task<T> GetAsync<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, ApiBodyTypeEnum bodyType = ApiBodyTypeEnum.Json)
        {
            return await ExecuteAsync<T>(url, ApiMethodEnum.GET, headers, queries, null, bodyType);
        }

        public T Post<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, object body = null, ApiBodyTypeEnum bodyType = ApiBodyTypeEnum.Json)
        {
            return Execute<T>(url, ApiMethodEnum.POST, headers, queries, body, bodyType);
        }

        public async Task<T> PostAsync<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, object body = null, ApiBodyTypeEnum bodyType = ApiBodyTypeEnum.Json)
        {
            return await ExecuteAsync<T>(url, ApiMethodEnum.POST, headers, queries, body, bodyType);
        }

        public T Put<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, object body = null, ApiBodyTypeEnum bodyType = ApiBodyTypeEnum.Json)
        {
            return Execute<T>(url, ApiMethodEnum.PUT, headers, queries, body, bodyType);
        }

        public async Task<T> PutAsync<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, object body = null, ApiBodyTypeEnum bodyType = ApiBodyTypeEnum.Json)
        {
            return await ExecuteAsync<T>(url, ApiMethodEnum.PUT, headers, queries, body, bodyType);
        }

        public T Patch<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, object body = null, ApiBodyTypeEnum bodyType = ApiBodyTypeEnum.Json)
        {
            return Execute<T>(url, ApiMethodEnum.PATCH, headers, queries, body, bodyType);
        }

        public async Task<T> PatchAsync<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, object body = null, ApiBodyTypeEnum bodyType = ApiBodyTypeEnum.Json)
        {
            return await ExecuteAsync<T>(url, ApiMethodEnum.PATCH, headers, queries, body, bodyType);
        }

        public T Delete<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, ApiBodyTypeEnum bodyType = ApiBodyTypeEnum.Json)
        {
            return Execute<T>(url, ApiMethodEnum.DELETE, headers, queries, null, bodyType);
        }

        public async Task<T> DeleteAsync<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, ApiBodyTypeEnum bodyType = ApiBodyTypeEnum.Json)
        {
            return await ExecuteAsync<T>(url, ApiMethodEnum.DELETE, headers, queries, null, bodyType);
        }
        
        public T Execute<T>(string url, ApiMethodEnum method, Dictionary<string, object> headers,
            Dictionary<string, object> queries, object body, ApiBodyTypeEnum bodyType = ApiBodyTypeEnum.Json)
        {
            var request = GetRestRequest(url, method.ToRestsharp(), headers, queries, body, bodyType.ToRestsharp());

            _logger.LogDebug("Executing {Method} {url}", method, url);
            Stopwatch watch = Stopwatch.StartNew();
            var response = _client.Execute<T>(request);
            watch.Stop();
            _logger.LogDebug("Executed {Method} {url} cost {time} ms", method, url, watch.ElapsedMilliseconds);
            _logger.LogInformation("Executed {Method} {url} return {Code}", method, url,
                response.StatusCode);
            _logger.LogDebug("Executed {Method} {url} return Content: {Content}", method, url,
                response.Content);

            if (response.ResponseStatus == ResponseStatus.Completed && (response.StatusCode == HttpStatusCode.OK ||
                                                                        response.StatusCode ==
                                                                        HttpStatusCode.NoContent))
            {
                return response.Data;
            }

            throw new ApiClientException(response.StatusCode, response.ErrorMessage, response.Content,
                response.ErrorException);
        }

        public async Task<T> ExecuteAsync<T>(string url, ApiMethodEnum method, Dictionary<string, object> headers,
            Dictionary<string, object> queries, object body, ApiBodyTypeEnum bodyType = ApiBodyTypeEnum.Json)
        {
            var request = GetRestRequest(url, method.ToRestsharp(), headers, queries, body, bodyType.ToRestsharp());

            _logger.LogDebug("Executing {Method} {url}", method, url);
            Stopwatch watch = Stopwatch.StartNew();
            var response = await _client.ExecuteAsync<T>(request);
            watch.Stop();
            _logger.LogDebug("Executed {Method} {url} cost {time} ms", method, url, watch.ElapsedMilliseconds);
            _logger.LogInformation("Executed {Method} {url} return {Code}", method, url,
                response.StatusCode);
            _logger.LogDebug("Executed {Method} {url} return Content: {Content}", method, url,
                response.Content);

            if (response.ResponseStatus == ResponseStatus.Completed && (response.StatusCode == HttpStatusCode.OK ||
                                                                        response.StatusCode ==
                                                                        HttpStatusCode.NoContent))
            {
                return response.Data;
            }

            throw new ApiClientException(response.StatusCode, response.ErrorMessage, response.Content,
                response.ErrorException);
        }
        
        #region Private

        private RestRequest GetRestRequest(string url, Method method, Dictionary<string, object> headers,
            Dictionary<string, object> queries, object body, DataFormat dataFormat)
        {
            var request = new RestRequest(url, method, dataFormat);

            if (headers != null && headers.Any())
            {
                foreach (var header in headers)
                {
                    request.AddHeader(header.Key, header.Value.ToString());
                }
            }

            if (queries != null && queries.Any())
            {
                foreach (var query in queries)
                {
                    request.AddQueryParameter(query.Key, query.Value.ToString());
                }
            }

            if (body != null)
            {
                switch (dataFormat)
                {
                    case DataFormat.Json:
                        request.AddJsonBody(body);
                        break;
                    case DataFormat.Xml:
                        request.AddXmlBody(body);
                        break;
                }
            }

            return request;
        }

        #endregion
    }
}