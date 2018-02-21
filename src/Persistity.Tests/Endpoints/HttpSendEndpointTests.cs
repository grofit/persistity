using System;
using System.Net.Http;
using System.Text;
using LazyData;
using Persistity.Endpoints.Files;
using Persistity.Endpoints.HttpEndpoint;
using Persistity.Tests.Models;
using Xunit;

namespace Persistity.Tests.Endpoints
{
    public class HttpSendEndpointTests
    {
        [Fact]
        public async void should_get_valid_response_from_endpoint()
        {
            var url = "https://httpbin.org/get";
            var dataObject = new DataObject("");
            
            var httpEndpoint = new HttpSendEndpoint(url);
            var httpResponse = (HttpResponseMessage)await httpEndpoint.Send(dataObject);
            
            Assert.True(httpResponse.IsSuccessStatusCode);
        }
        
        [Fact]
        public async void should_correctly_post_data()
        {
            var url = "https://httpbin.org/post";
            var someJson = "{ 'test': 'test' }";
                
            var dataObject = new DataObject(someJson);
            
            var httpEndpoint = new HttpSendEndpoint(url, HttpMethod.Post);
            var httpResponse = (HttpResponseMessage)await httpEndpoint.Send(dataObject);
            
            Assert.True(httpResponse.IsSuccessStatusCode);

            var content = await httpResponse.Content.ReadAsStringAsync();
            Assert.Contains(someJson, content);
        }
    }
}