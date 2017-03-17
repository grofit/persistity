using System;
using System.IO;

namespace Persistity.Endpoints.Files
{
    public class WriteFile : ISendData<string>, ISendData<byte[]>
    {
        public string Filename { get; set; }

        public WriteFile(string filename)
        {
            Filename = filename;
        }

        public void Execute(string data, Action onSuccess, Action<Exception> onError)
        {
            try
            { File.WriteAllText(Filename, data); }
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
            { File.WriteAllBytes(Filename, data); }
            catch (Exception ex)
            {
                onError(ex);
                return;
            }

            onSuccess();
        }
    }
}