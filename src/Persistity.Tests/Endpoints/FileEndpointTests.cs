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

        [Fact]
        public async void should_correctly_overwrite_different_file_lengths()
        {
            var filename = "dummy-binary-file-different-lengths.bin";

            var expectedString1 = "This is what was requested";
            Console.WriteLine("default: {0}", expectedString1);

            var fileEndpoint = new FileEndpoint(filename);
            var dataObject1 = new DataObject(expectedString1);

            await fileEndpoint.Send(dataObject1);
            var data1 = await fileEndpoint.Receive();

            Assert.Equal(expectedString1, data1.AsString);

            var expectedString2 = expectedString1 + expectedString1;
            Console.WriteLine("default: {0}", expectedString2);

            fileEndpoint = new FileEndpoint(filename);
            var dataObject2 = new DataObject(expectedString2);

            await fileEndpoint.Send(dataObject2);
            var data2 = await fileEndpoint.Receive();

            Assert.Equal(expectedString2, data2.AsString);

            var expectedString3 = "A shorter string";
            Console.WriteLine("default: {0}", expectedString3);

            fileEndpoint = new FileEndpoint(filename);
            var dataObject3 = new DataObject(expectedString3);

            await fileEndpoint.Send(dataObject3);
            var data3 = await fileEndpoint.Receive();

            Assert.Equal(expectedString3, data3.AsString);
        }



    }
}