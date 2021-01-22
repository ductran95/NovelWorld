using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using NovelWorld.Data.Constants;
using NovelWorld.Data.DTO;

namespace NovelWorld.Utility.Exceptions
{
    public class ApiClientException: DomainException
    {
        public int StatusCode { get; private set; }
        public string Content { get; private set; }
        public ApiClientException(int statusCode, string message = "", string content = "", Exception innerException = null) : base(!string.IsNullOrEmpty(message) ? message: content, innerException)
        {
            StatusCode = statusCode;
            Content = content;
        }

        public override HttpException WrapException()
        {
            var errorResponse = new List<Error>();

            // Get error from GraphResponse
            try
            {
                using (JsonDocument document = JsonDocument.Parse(Content))
                {
                    JsonElement root = document.RootElement;
                    JsonElement responseErrors = root.GetProperty("errors");
                    foreach (JsonElement responseError in responseErrors.EnumerateArray())
                    {
                        var code = responseError.GetProperty("code").GetString();
                        var message = responseError.GetProperty("message").GetString();
                        errorResponse.Add(new Error(code, message));
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            // Only return ApiClientError when response does not have any
            if (!errorResponse.Any())
            {
                errorResponse.Add(new Error(CommonErrorCodes.ApiClientError, $"Code: {this.StatusCode}, Message: {this.Message}, Content: {this.Content}"));
            }

            return new HttpException(this.StatusCode, errorResponse, this.Message, this);
        }
    }
}
