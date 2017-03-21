using System;
using System.IO;

namespace Persistity.Endpoints.Files
{
    public class WriteFile : ISendData<string>, ISendData<byte[]>
    {
        public string FilePath { get; set; }

        public WriteFile(string filePath)
        {
            FilePath = filePath;
        }

        public void Execute(string data, Action onSuccess, Action<Exception> onError)
        {
            try
            { File.WriteAllText(FilePath, data); }
            catch (Exception ex)
            {
                onError(ex);
                return;
            }
            onSuccess();
        }

        public void Execute(byte[] data, Action onSuccess, Action<Exception> onError)
        {
            try
            { File.WriteAllBytes(FilePath, data); }
            catch (Exception ex)
            {
                onError(ex);
                return;
            }

            onSuccess();
        }
    }
}