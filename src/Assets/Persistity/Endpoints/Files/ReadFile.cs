using System;
using System.IO;

namespace Persistity.Endpoints.Files
{
    public class ReadFile : IReceiveData<string>, IReceiveData<byte[]>
    {
        public string Filename { get; set; }

        public ReadFile(string filename)
        {
            Filename = filename;
        }

        public void Execute(Action<string> onSuccess, Action<Exception> onError)
        {
            string data;
            try
            {
                data = File.ReadAllText(Filename);
            }
            catch (Exception ex)
            {
                onError(ex);
                return;
            }
            onSuccess(data);
        }

        public void Execute(Action<byte[]> onSuccess, Action<Exception> onError)
        {
            byte[] data;
            try
            {
                data = File.ReadAllBytes(Filename);
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