using System;
using System.IO;
using System.Threading.Tasks;
using LazyData;

namespace Persistity.Endpoints.Files
{
    public class FileEndpoint : IReceiveDataEndpoint, ISendDataEndpoint
    {
        public string FilePath { get; }

        public FileEndpoint(string filePath)
        { FilePath = filePath; }

        public async Task<DataObject> Receive()
        {
            using (var reader = File.OpenRead(FilePath))
            {
                var byteData = new byte[reader.Length];
                await reader.ReadAsync(byteData, 0, byteData.Length);
                return new DataObject(byteData);
            }
        }
        
        public async Task<object> Send(DataObject data)
        {
            using (var writer = File.Open(FilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var bytes = data.AsBytes;
                await writer.WriteAsync(bytes, 0, bytes.Length);
                return Task.FromResult<object>(null);
            }
        }
    }
}