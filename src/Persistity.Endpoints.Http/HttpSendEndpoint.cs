using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Persistity.Core;
using Persistity.Core.Data;

namespace Persistity.Endpoints.Http
{
    public class HttpSendEndpoint : ISendDataEndpoint, IDisposable
    {
        public HttpMethod Method { get; }
        public IDictionary<string, IEnumerable<string>> Headers { get; }
       
        public string Url { get; }
        public string MimeType { get; }
            
        private readonly HttpClient _httpClient = new HttpClient();

        public HttpSendEndpoint(string url, HttpMethod method = null, string mimeType = "application/json", 
            IDictionary<string, IEnumerable<string>> headers = null)
        {
            Url = url;
            MimeType = mimeType;
            Method = method ?? HttpMethod.Get;
            Headers = headers ?? new Dictionary<string, IEnumerable<string>>();
        }
        
        public async Task<object> Send(DataObject data)
        {
            using (var httpRequest = new HttpRequestMessage(Method, Url))
            {
                foreach (var header in Headers)
                { httpRequest.Headers.Add(header.Key, header.Value); }

                httpRequest.Content = GenerateContent(data);
                
                return await _httpClient.SendAsync(httpRequest);
            }
        }

        public virtual HttpContent GenerateContent(DataObject data)
        { return new StringContent(data.AsString, Encoding.UTF8, MimeType); }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}