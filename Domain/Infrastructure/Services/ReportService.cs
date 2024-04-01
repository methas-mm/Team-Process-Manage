using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.IO;

namespace Infrastructure.Services
{
    public class ReportService : IReportService
    {
        public HttpClient Client { get; }
        public ReportService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            var bearerToken = httpContextAccessor.HttpContext.Request
                              .Headers["Authorization"]
                              .FirstOrDefault(h => h.StartsWith("bearer ", StringComparison.InvariantCultureIgnoreCase));

            // Add authorization if found
            if (bearerToken != null)
                client.DefaultRequestHeaders.Add("Authorization", bearerToken);

            Client = client;
        }

        public async Task<T> GetAsync<T>(string requestUrl)
        {
            var response = await Client.GetAsync(requestUrl);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<T>();

            return result;
        }

        public async Task<byte[]> GetAsByteArrayAsync(string requestUrl)
        {
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await Client.GetAsync(requestUrl);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsByteArrayAsync();
        }
        public async Task<T> PostAsync<T>(string requestUrl, object bodyContent)
        {
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(this.ConvertToContent(bodyContent), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync(requestUrl, content);

            response.EnsureSuccessStatusCode();

            if (typeof(T) == typeof(String))
            {
                var result = await response.Content.ReadAsStringAsync();
                return (T)Convert.ChangeType(result, typeof(T));
            }
            else if (response.Content.Headers.ContentType.MediaType.Equals("text/html"))
            {
                var result = await response.Content.ReadAsStringAsync();
                return (T)Convert.ChangeType(result, typeof(String));
            }
            else if (response.Content.Headers.ContentType.MediaType.Equals("application/json"))
            {
                var result = await response.Content.ReadAsStringAsync();
                return (T)Convert.ChangeType(result, typeof(String));
            }
            else
            {
                var result = await response.Content.ReadAsAsync<T>();
                return result;
            }
        }

        public async Task<byte[]> PostAsByteArrayAsync(string requestUrl, object bodyContent)
        {
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(this.ConvertToContent(bodyContent), Encoding.UTF8, "application/json");
            
                var response = await Client.PostAsync(requestUrl, content);

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsByteArrayAsync();
            

        }

        public async Task<string> GetAsyncReturnString(string requestUrl)
        {
            var response = await Client.GetAsync(requestUrl);
            return response.StatusCode.ToString();
        }


        private string ConvertToContent(object content)
        {
            return JsonConvert.SerializeObject(content);
        }

        public async Task<dynamic> PostAsAsync(string requestUrl, object bodyContent)
        {
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(this.ConvertToContent(bodyContent), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync(requestUrl, content);

            response.EnsureSuccessStatusCode();
            return response;
        }
    }
}
