using System;
using System.IO;

namespace Persistity.Endpoints.Files
{
    public class ReadFile : IReceiveData
    {
        public string FilePath { get; set; }

        public ReadFile(string filePath)
        {
            FilePath = filePath;
        }

        public void Execute(Action<byte[]> onSuccess, Action<Exception> onError)
        {
            byte[] data;
            try
            {
                data = File.ReadAllBytes(FilePath);
            }
            catch (Exception ex)
            {
                onError(ex);
                return;
            }
            onSuccess(data);
        }
    }
}