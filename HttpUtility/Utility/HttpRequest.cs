using HttpUtility.Interface;
using HttpUtility.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace HttpUtility.Utility
{
    public class HttpRequest : IHttpRequest
    {
        private string _baseUrl;
        public string BaseUrl { get => _baseUrl; set => _baseUrl = value; }
        private string _token = null;
        public string Token { get => _token; set => _token = value; }

        private HttpClient httpClient;

        public HttpRequest(string baseUrl, string token = null)
        {
            this.BaseUrl = baseUrl;
            this.Token = token;

            this.httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri(baseUrl);

            if (token != null) { httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}"); }
        }

        public async Task<ResponseResult<TResult>> GetAsync<TResult>(string url)
        {
            HttpResponseMessage responseMessage = await httpClient.GetAsync(url);

            string rawContent = await responseMessage.Content.ReadAsStringAsync();

            ResponseResult<TResult> responseResult = new ResponseResult<TResult>
            {
                IsSuccess = responseMessage.IsSuccessStatusCode,
                StatusCode = (int)responseMessage.StatusCode,
                Message = responseMessage.ReasonPhrase,
                RawContent = rawContent
            };

            if (responseMessage.IsSuccessStatusCode && !string.IsNullOrWhiteSpace(rawContent))
            {
                responseResult.Data = JsonConvert.DeserializeObject<TResult>(rawContent);
            }

            return responseResult;
        }

        public async Task<ResponseResult<TResult>> GetAsync<TResult>(string url, Dictionary<string, string> urlParam = null)
        {
            url = buildQueryString(url, urlParam);

            ResponseResult<TResult> responseResult = await GetAsync<TResult>(url);

            return responseResult;
        }

        public async Task<ResponseResult<TResult>> PostAsync<TResult>(string url, object input)
        {
            HttpContent content = null;
            if (input != null)
            {
                string reqBody = JsonConvert.SerializeObject(input);
                content = new StringContent(reqBody, System.Text.Encoding.UTF8, "application/json");
            }

            HttpResponseMessage responseMessage = await httpClient.PostAsync(url, content);
            string rawContent = await responseMessage.Content.ReadAsStringAsync();

            ResponseResult<TResult> responseResult = new ResponseResult<TResult>
            {
                IsSuccess = responseMessage.IsSuccessStatusCode,
                StatusCode = (int)responseMessage.StatusCode,
                Message = responseMessage.ReasonPhrase,
                RawContent = rawContent
            };

            if (responseMessage.IsSuccessStatusCode && !string.IsNullOrWhiteSpace(rawContent))
            {
                responseResult.Data = JsonConvert.DeserializeObject<TResult>(rawContent);
            }

            return responseResult;
        }

        public async Task<ResponseResult<TResult>> PostAsync<TResult>(string url, object input = null, Dictionary<string, string> urlParam = null)
        {
            url = buildQueryString(url, urlParam);

            ResponseResult<TResult> responseResult = await PostAsync<TResult>(url, input);

            return responseResult;
        }

        public async Task<ResponseResult<TResult>> PostAsync<TResult>(string url, MultipartFormDataContent input, Dictionary<string, string> urlParam = null)
        {
            url = buildQueryString(url, urlParam);

            HttpResponseMessage responseMessage = await httpClient.PostAsync(url, input);
            string rawContent = await responseMessage.Content.ReadAsStringAsync();

            ResponseResult<TResult> responseResult = new ResponseResult<TResult>
            {
                IsSuccess = responseMessage.IsSuccessStatusCode,
                StatusCode = (int)responseMessage.StatusCode,
                Message = responseMessage.ReasonPhrase,
                RawContent = rawContent
            };

            if (responseMessage.IsSuccessStatusCode && !string.IsNullOrWhiteSpace(rawContent))
            {
                responseResult.Data = JsonConvert.DeserializeObject<TResult>(rawContent);
            }

            return responseResult;
        }

        public async Task<ResponseResult<TResult>> PutAsync<TResult>(string url, object input)
        {
            string reqBody = JsonConvert.SerializeObject(input);
            HttpContent content = new StringContent(reqBody);

            HttpResponseMessage responseMessage = await httpClient.PutAsync(url, content);
            string rawContent = await responseMessage.Content.ReadAsStringAsync();

            ResponseResult<TResult> responseResult = new ResponseResult<TResult>
            {
                IsSuccess = responseMessage.IsSuccessStatusCode,
                StatusCode = (int)responseMessage.StatusCode,
                Message = responseMessage.ReasonPhrase,
                RawContent = rawContent
            };

            if (responseMessage.IsSuccessStatusCode && !string.IsNullOrWhiteSpace(rawContent))
            {
                responseResult.Data = JsonConvert.DeserializeObject<TResult>(rawContent);
            }

            return responseResult;
        }

        public async Task<ResponseResult<TResult>> PutAsync<TResult>(string url, object input, Dictionary<string, string> urlParam = null)
        {
            url = buildQueryString(url, urlParam);

            ResponseResult<TResult> responseResult = await PutAsync<TResult>(url, input);

            return responseResult;
        }

        public async Task<ResponseResult<TResult>> PutAsync<TResult>(string url, HttpContent content)
        {
            HttpResponseMessage responseMessage = await httpClient.PostAsync(url, content);
            string rawContent = await responseMessage.Content.ReadAsStringAsync();

            ResponseResult<TResult> responseResult = new ResponseResult<TResult>
            {
                IsSuccess = responseMessage.IsSuccessStatusCode,
                StatusCode = (int)responseMessage.StatusCode,
                Message = responseMessage.ReasonPhrase,
                RawContent = rawContent
            };

            if (responseMessage.IsSuccessStatusCode && !string.IsNullOrWhiteSpace(rawContent))
            {
                responseResult.Data = JsonConvert.DeserializeObject<TResult>(rawContent);
            }

            return responseResult;
        }

        public async Task<ResponseResult> DeleteAsync(string url)
        {
            HttpResponseMessage responseMessage = await httpClient.DeleteAsync(url);
            string rawContent = await responseMessage.Content.ReadAsStringAsync();

            ResponseResult responseResult = new ResponseResult
            {
                IsSuccess = responseMessage.IsSuccessStatusCode,
                StatusCode = (int)responseMessage.StatusCode,
                Message = responseMessage.ReasonPhrase,
                RawContent = rawContent
            };

            return responseResult;
        }

        public async Task<ResponseResult> DeleteAsync(string url, Dictionary<string, string> urlParam = null)
        {
            url = buildQueryString(url, urlParam);

            ResponseResult responseResult = await DeleteAsync(url);

            return responseResult;
        }

        public async Task<ResponseResult<TResult>> PatchAsync<TResult>(string url, object input)
        {
            string reqBody = JsonConvert.SerializeObject(input);

            var patchMethod = new HttpMethod("PATCH");

            var request = new HttpRequestMessage(patchMethod, url)
            {
                Content = new StringContent(reqBody, Encoding.UTF8, "application/json")
            };

            HttpResponseMessage responseMessage = await httpClient.SendAsync(request);
            string rawContent = await responseMessage.Content.ReadAsStringAsync();


            ResponseResult<TResult> responseResult = new ResponseResult<TResult>
            {
                IsSuccess = responseMessage.IsSuccessStatusCode,
                StatusCode = (int)responseMessage.StatusCode,
                Message = responseMessage.ReasonPhrase,
                RawContent = rawContent
            };

            if (responseMessage.IsSuccessStatusCode && !string.IsNullOrWhiteSpace(rawContent))
            {
                responseResult.Data = JsonConvert.DeserializeObject<TResult>(rawContent);
            }

            return responseResult;
        }

        public async Task<ResponseResult<TResult>> PatchAsync<TResult>(string url, object input, Dictionary<string, string> urlParam = null)
        {
            url = buildQueryString(url, urlParam);

            ResponseResult<TResult> responseResult = await PatchAsync<TResult>(url, input);

            return responseResult;
        }

        public async Task<ResponseResult<TResult>> PatchAsync<TResult>(string url, MultipartFormDataContent input, Dictionary<string, string> urlParam = null)
        {
            var patchMethod = new HttpMethod("PATCH");

            var request = new HttpRequestMessage(patchMethod, url)
            {
                Content = input,
            };

            HttpResponseMessage responseMessage = await httpClient.SendAsync(request);
            string rawContent = await responseMessage.Content.ReadAsStringAsync();

            ResponseResult<TResult> responseResult = new ResponseResult<TResult>
            {
                IsSuccess = responseMessage.IsSuccessStatusCode,
                StatusCode = (int)responseMessage.StatusCode,
                Message = responseMessage.ReasonPhrase,
                RawContent = rawContent
            };

            if (responseMessage.IsSuccessStatusCode && !string.IsNullOrWhiteSpace(rawContent))
            {
                responseResult.Data = JsonConvert.DeserializeObject<TResult>(rawContent);
            }

            return responseResult;
        }

        private string buildQueryString(string url, Dictionary<string, string> urlParam)
        {
            if (urlParam == null) return url;

            string parameter = "?";
            url += parameter;

            foreach (var kvp in urlParam)
            {
                url += $"{kvp.Key}={kvp.Value}&";
            }

            url = url.TrimEnd('&');

            return url;
        }
    }
}
