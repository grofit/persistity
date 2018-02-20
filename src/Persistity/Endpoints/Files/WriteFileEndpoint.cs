using System;
using System.IO;
using LazyData;

namespace Persistity.Endpoints.Files
{
    public class WriteFileEndpoint : ISendDataEndpoint
    {
        public string FilePath { get; set; }

        public WriteFileEndpoint(string filePath)
        {
            FilePath = filePath;
        }

        public void Execute(DataObject data, Action<object> onSuccess, Action<Exception> onError)
        {
            try
            { File.WriteAllBytes(FilePath, data.AsBytes); }
            catch (Exception ex)
            {
                onError(ex);
                return;
            }

            onSuccess(null);
        }
    }
}