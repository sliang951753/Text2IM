using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace Text2Mail.Services
{
    class RESTfulService<T>
    {
        private Uri _serverUrl;

        public RESTfulService(string serverUrl)
        {
            _serverUrl = new Uri(serverUrl);
        }

        protected async Task<List<T>> GetAll(string resourceRoute)
        {
            var items = new List<T>();
            var requestUri = new Uri(_serverUrl, resourceRoute);

            using(var httpClient = new HttpClient() { MaxResponseContentBufferSize = 256*1024 })
            {
                var response = await httpClient.GetAsync(requestUri);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<T>>(json);
                }
            }

            return items;
        }

        protected async Task<List<T>> GetSpecific(string resourceRoute, string queryString)
        {
            var items = new List<T>();
            var builder = new UriBuilder(new Uri(_serverUrl, resourceRoute));
            builder.Query = queryString;
            var requestUri = builder.Uri;

            using (var httpClient = new HttpClient() { MaxResponseContentBufferSize = 256 * 1024 })
            {
                var response = await httpClient.GetAsync(requestUri);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<T>>(json);
                }
            }

            return items;
        }


        private async Task<bool> CreateInternal(Uri requestUri, string json)
        {
            using (var httpClient = new HttpClient() { MaxResponseContentBufferSize = 256 * 1024 })
            {

                try
                {
                    var response = await httpClient.PostAsync(requestUri,
                    new StringContent(json, Encoding.UTF8, "application/json"));

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
                catch (TaskCanceledException e)
                {
                    
                }
            }

            return false;
        }

        protected async Task<bool> Create(string resourceRoute, IEnumerable<T> data)
        {
            var json = JsonConvert.SerializeObject(data);
            var requestUri = new Uri(_serverUrl, resourceRoute);

            return await CreateInternal(requestUri, json);
        }

        protected async Task<bool> Create(string resourceRoute, T data)
        {
            var json = JsonConvert.SerializeObject(data);
            var requestUri = new Uri(_serverUrl, resourceRoute);

            return await CreateInternal(requestUri, json);
        }

    }
}