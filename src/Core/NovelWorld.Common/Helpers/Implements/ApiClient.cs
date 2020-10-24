using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NovelWorld.Common.Exceptions;
using NovelWorld.Common.Helpers.Abstractions;
using RestSharp;

namespace NovelWorld.Common.Helpers.Implements
{
    public class ApiClient : IApiClient
    {
        //private  readonly IRestClient _client = new RestClient().UseSystemTextJson(new JsonSerializerOptions 
        //{ 
        //    //PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        //    PropertyNameCaseInsensitive = true
        //});

        private readonly IRestClient _client;
        //     = new RestClient().UseNewtonsoftJson(new JsonSerializerSettings
        // {
        //     ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        // });

        private readonly ILogger<ApiClient> _logger;

        public ApiClient(ILogger<ApiClient> logger, IRestClient client)
        {
            _logger = logger;
            _client = client;
        }

        public T Get<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, DataFormat dataFormat = DataFormat.Json)
        {
            return Execute<T>(url, Method.GET, headers, queries, null, dataFormat);
        }

        public async Task<T> GetAsync<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, DataFormat dataFormat = DataFormat.Json)
        {
            return await ExecuteAsync<T>(url, Method.GET, headers, queries, null, dataFormat);
        }

        public T Post<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, object body = null, DataFormat dataFormat = DataFormat.Json)
        {
            return Execute<T>(url, Method.POST, headers, queries, body, dataFormat);
        }

        public async Task<T> PostAsync<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, object body = null, DataFormat dataFormat = DataFormat.Json)
        {
            return await ExecuteAsync<T>(url, Method.POST, headers, queries, body, dataFormat);
        }

        public T Put<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, object body = null, DataFormat dataFormat = DataFormat.Json)
        {
            return Execute<T>(url, Method.PUT, headers, queries, body, dataFormat);
        }

        public async Task<T> PutAsync<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, object body = null, DataFormat dataFormat = DataFormat.Json)
        {
            return await ExecuteAsync<T>(url, Method.PUT, headers, queries, body, dataFormat);
        }

        public T Patch<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, object body = null, DataFormat dataFormat = DataFormat.Json)
        {
            return Execute<T>(url, Method.PATCH, headers, queries, body, dataFormat);
        }

        public async Task<T> PatchAsync<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, object body = null, DataFormat dataFormat = DataFormat.Json)
        {
            return await ExecuteAsync<T>(url, Method.PATCH, headers, queries, body, dataFormat);
        }

        public T Delete<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, DataFormat dataFormat = DataFormat.Json)
        {
            return Execute<T>(url, Method.DELETE, headers, queries, null, dataFormat);
        }

        public async Task<T> DeleteAsync<T>(string url, Dictionary<string, object> headers = null,
            Dictionary<string, object> queries = null, DataFormat dataFormat = DataFormat.Json)
        {
            return await ExecuteAsync<T>(url, Method.DELETE, headers, queries, null, dataFormat);
        }

        #region Private

        private T Execute<T>(string url, Method method, Dictionary<string, object> headers,
            Dictionary<string, object> queries, object body, DataFormat dataFormat)
        {
            var request = GetRestRequest(url, method, headers, queries, body, dataFormat);

            _logger.LogTrace("Executing {Method} {url}", method, url);
            Stopwatch watch = Stopwatch.StartNew();
            var response = _client.Execute<T>(request);
            watch.Stop();
            _logger.LogTrace("Executed {Method} {url} cost {time} ms", method, url, watch.ElapsedMilliseconds);
            _logger.LogInformation("Execute {Method} {url} return {Code}; Content: {Content}", method, url,
                response.StatusCode, response.Content);

            if (response.ResponseStatus == ResponseStatus.Completed && (response.StatusCode == HttpStatusCode.OK ||
                                                                        response.StatusCode ==
                                                                        HttpStatusCode.NoContent))
            {
                return response.Data;
            }

            throw new ApiClientException(response.StatusCode, response.ErrorMessage, response.Content,
                response.ErrorException);
        }

        private async Task<T> ExecuteAsync<T>(string url, Method method, Dictionary<string, object> headers,
            Dictionary<string, object> queries, object body, DataFormat dataFormat)
        {
            var request = GetRestRequest(url, method, headers, queries, body, dataFormat);

            _logger.LogTrace("Executing {Method} {url}", method, url);
            Stopwatch watch = Stopwatch.StartNew();
            var response = await _client.ExecuteAsync<T>(request);
            watch.Stop();
            _logger.LogTrace("Execute {Method} {url} cost {time} ms", method, url, watch.ElapsedMilliseconds);
            _logger.LogInformation("Execute {Method} {url} return {Code}; Content: {Content}", method, url,
                response.StatusCode, response.Content);

            if (response.ResponseStatus == ResponseStatus.Completed && (response.StatusCode == HttpStatusCode.OK ||
                                                                        response.StatusCode ==
                                                                        HttpStatusCode.NoContent))
            {
                return response.Data;
            }

            throw new ApiClientException(response.StatusCode, response.ErrorMessage, response.Content,
                response.ErrorException);
        }

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