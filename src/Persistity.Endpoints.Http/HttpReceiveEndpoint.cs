using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Persistity.Core;


namespace Persistity.Endpoints.Http
{
    public class HttpReceiveEndpoint : IReceiveDataEndpoint, IDisposable
    {
        public HttpMethod Method { get; }
        public IDictionary<string, IEnumerable<string>> Headers { get; }

        public string Url { get; }
        public string MimeType { get; }
        public string Content { get; }

        private readonly HttpClient _httpClient = new HttpClient();

        public HttpReceiveEndpoint(string url, HttpMethod method = null, string mimeType = "application/json", string content = null,
            IDictionary<string, IEnumerable<string>> headers = null)
        {
            Url = url;
            MimeType = mimeType;
            Method = method ?? HttpMethod.Get;
            Content = content;
            Headers = headers ?? new Dictionary<string, IEnumerable<string>>();
        }

        public virtual HttpContent GenerateContent()
        { return new StringContent(Content, Encoding.UTF8, MimeType); }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        public async Task<DataObject> Receive()
        {
            using (var httpRequest = new HttpRequestMessage(Method, Url))
            {
                foreach (var header in Headers)
                { httpRequest.Headers.Add(header.Key, header.Value); }

                if (Content != null)
                { httpRequest.Content = GenerateContent(); }

                var response = await _httpClient.SendAsync(httpRequest);
                var contentData = await response.Content.ReadAsStringAsync();
                return new DataObject(contentData);
            }
        }
    }
}