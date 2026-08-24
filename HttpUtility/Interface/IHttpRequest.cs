using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HttpUtility.Model;

namespace HttpUtility.Interface
{
    public interface IHttpRequest
    {
        Task<ResponseResult<TResult>> GetAsync<TResult>(string url);
        Task<ResponseResult<TResult>> GetAsync<TResult>(string url, Dictionary<string, string> urlParam = null);
        Task<ResponseResult<TResult>> PostAsync<TResult>(string url, object input);
        Task<ResponseResult<TResult>> PostAsync<TResult>(string url, object input = null, Dictionary<string, string> urlParam = null);
        Task<ResponseResult<TResult>> PostAsync<TResult>(string url, MultipartFormDataContent input, Dictionary<string, string> urlParam = null);

        Task<ResponseResult<TResult>> PatchAsync<TResult>(string url, object input);
        Task<ResponseResult<TResult>> PatchAsync<TResult>(string url, object input, Dictionary<string, string> urlParam = null);
        Task<ResponseResult<TResult>> PatchAsync<TResult>(string url, MultipartFormDataContent input, Dictionary<string, string> urlParam = null);

        Task<ResponseResult<TResult>> PutAsync<TResult>(string url, object input);
        Task<ResponseResult<TResult>> PutAsync<TResult>(string url, object input, Dictionary<string, string> urlParam = null);
        Task<ResponseResult<TResult>> PutAsync<TResult>(string url, HttpContent content);

        Task<ResponseResult> DeleteAsync(string url);
        Task<ResponseResult> DeleteAsync(string url, Dictionary<string, string> urlParam = null);

        String BaseUrl { set; }
        String Token { set; get; }
        //String SecretKey { set; get; }
        //Credential Credential { set; }
    }
}
