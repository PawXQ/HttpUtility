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

        public async Task<string> GetAsync(string url)
        {
            HttpResponseMessage responseMessage = await httpClient.GetAsync(url);

            responseMessage.EnsureSuccessStatusCode();

            string response = await responseMessage.Content.ReadAsStringAsync();

            return response;
        }

        public async Task<TResult> GetAsync<TResult>(string url, Dictionary<string, string> urlParam = null)
        {
            url = buildQueryString(url, urlParam);

            string response = await GetAsync(url);

            return JsonConvert.DeserializeObject<TResult>(response);
        }

        public async Task<string> PostAsync(string url, object input)
        {
            HttpContent content;
            if (input == null)
            {
                content = null;
            }
            else
            {
                string reqBody = JsonConvert.SerializeObject(input);
                content = new StringContent(reqBody);
            }

            HttpResponseMessage responseMessage = await httpClient.PostAsync(url, content);

            responseMessage.EnsureSuccessStatusCode();

            string response = await responseMessage.Content.ReadAsStringAsync();

            return response;
        }

        public async Task<TResult> PostAsync<TResult>(string url, object input = null, Dictionary<string, string> urlParam = null)
        {
            url = buildQueryString(url, urlParam);

            string response = await PostAsync(url, input);

            return JsonConvert.DeserializeObject<TResult>(response);
        }

        public Task<TResult> PostAsync<TResult>(string url, MultipartFormDataContent input, Dictionary<string, string> urlParam = null)
        {
            throw new NotImplementedException();
        }

        public async Task<string> PutAsync(string url, object input)
        {
            string reqBody = JsonConvert.SerializeObject(input);
            HttpContent content = new StringContent(reqBody);

            HttpResponseMessage responseMessage = await httpClient.PutAsync(url, content);

            responseMessage.EnsureSuccessStatusCode();

            string response = await responseMessage.Content.ReadAsStringAsync();

            return response;
        }

        public async Task<TResult> PutAsync<TResult>(string url, object input, Dictionary<string, string> urlParam = null)
        {
            url = buildQueryString(url, urlParam);

            string response = await PutAsync(url, input);

            return JsonConvert.DeserializeObject<TResult>(response);
        }

        public async Task<string> PutAsync(string url, HttpContent content)
        {
            HttpResponseMessage responseMessage = await httpClient.PutAsync(url, content);

            responseMessage.EnsureSuccessStatusCode();

            string response = await responseMessage.Content.ReadAsStringAsync();

            return response;
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            HttpResponseMessage responseMessage = await httpClient.DeleteAsync(url);

            responseMessage.EnsureSuccessStatusCode();

            return responseMessage;
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url, Dictionary<string, string> urlParam = null)
        {
            url = buildQueryString(url, urlParam);

            HttpResponseMessage responseMessage = await httpClient.DeleteAsync(url);

            responseMessage.EnsureSuccessStatusCode();

            return responseMessage;
        }

        //public async Task<DeleteResult> DeleteAsync(string url, Dictionary<string, string> urlParam = null)
        //{
        //    url = buildQueryString(url, urlParam);

        //    HttpResponseMessage responseMessage = await httpClient.DeleteAsync(url);

        //    return new DeleteResult
        //    {
        //        IsSuccess = responseMessage.IsSuccessStatusCode,
        //        StatusCode = (int)responseMessage.StatusCode,
        //        Message = responseMessage.ReasonPhrase.ToString(),
        //    };
        //}

        public async Task<string> PatchAsync(string url, object input)
        {
            string reqBody = JsonConvert.SerializeObject(input);

            var patchMethod = new HttpMethod("PATCH");

            var request = new HttpRequestMessage(patchMethod, url)
            {
                Content = new StringContent(reqBody, Encoding.UTF8, "application/json")
            };

            HttpResponseMessage responseMessage = await httpClient.SendAsync(request);

            responseMessage.EnsureSuccessStatusCode();

            string response = await responseMessage.Content.ReadAsStringAsync();

            return response;
        }

        public async Task<TResult> PatchAsync<TResult>(string url, object input, Dictionary<string, string> urlParam = null)
        {
            url = buildQueryString(url, urlParam);

            string response = await PatchAsync(url, input);

            return JsonConvert.DeserializeObject<TResult>(response);
        }

        public Task<TResult> PatchAsync<TResult>(string url, MultipartFormDataContent input, Dictionary<string, string> urlParam = null)
        {
            throw new NotImplementedException();
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
