using System;
using System.Text;
using LazyData;
using Persistity.Endpoints.Files;
using Xunit;

namespace Persistity.Tests.Endpoints
{
    public class FileEndpointTests
    {
        [Fact]
        public async void should_correctly_read_and_write_bytes()
        {
            var filename = "dummy-binary-file.bin";
            
            var expectedBytes = Encoding.UTF8.GetBytes("This is what was requested");
            Console.WriteLine("default: {0}", BitConverter.ToString(expectedBytes));

            var fileEndpoint = new FileEndpoint(filename);
            var dataObject = new DataObject(expectedBytes);
            
            await fileEndpoint.Send(dataObject);
            var data = await fileEndpoint.Receive();
            
            Assert.Equal(expectedBytes, data.AsBytes);
        }
        
        [Fact]
        public async void should_correctly_read_and_write_string()
        {
            var filename = "dummy-binary-file.bin";
            
            var expectedString = "This is what was requested";
            Console.WriteLine("default: {0}", expectedString);

            var fileEndpoint = new FileEndpoint(filename);
            var dataObject = new DataObject(expectedString);
            
            await fileEndpoint.Send(dataObject);
            var data = await fileEndpoint.Receive();
            
            Assert.Equal(expectedString, data.AsString);
        }
    }
}