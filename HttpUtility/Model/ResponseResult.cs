using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace HttpUtility.Model
{
    public class ResponseResult<T>
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string RawContent { get; set; }
        public Dictionary<string, string> Headers { get; set; }

        public T Data { get; set; }

        public ResponseResult() { }
        public ResponseResult(HttpResponseMessage httpResponseMessage, string rawContent)
        {
            IsSuccess = httpResponseMessage.IsSuccessStatusCode;
            StatusCode = (int)httpResponseMessage.StatusCode;
            Message = httpResponseMessage.ReasonPhrase;
            RawContent = rawContent;
            Headers = httpResponseMessage.Headers.ToDictionary(
                h => h.Key,
                h => string.Join(", ", h.Value), StringComparer.OrdinalIgnoreCase
            );
        }
    }

    public class ResponseResult
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string RawContent { get; set; }
        public Dictionary<string, string> Headers { get; set; }


        public ResponseResult() { }
        public ResponseResult(HttpResponseMessage httpResponseMessage, string rawContent)
        {
            IsSuccess = httpResponseMessage.IsSuccessStatusCode;
            StatusCode = (int)httpResponseMessage.StatusCode;
            Message = httpResponseMessage.ReasonPhrase;
            RawContent = rawContent;
            Headers = httpResponseMessage.Headers.ToDictionary(
                h => h.Key,
                h => string.Join(", ", h.Value), StringComparer.OrdinalIgnoreCase
            );
        }
    }
}
