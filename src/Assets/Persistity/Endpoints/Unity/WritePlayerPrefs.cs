using System;
using System.Text;
using UnityEngine;

namespace Persistity.Endpoints.Unity
{
    public class WritePlayerPrefs : ISendData<string>, ISendData<byte[]>
    {
        public string KeyName { get; set; }

        public WritePlayerPrefs(string keyName)
        {
            KeyName = keyName;
        }

        public void Execute(string data, Action onSuccess, Action<Exception> onError)
        {
            try
            { PlayerPrefs.SetString(KeyName, data); }
            catch (Exception ex)
            {
                onError(ex);
                return;
            }
            onSuccess();
        }

        public void Execute(byte[] data, Action onSuccess, Action<Exception> onError)
        {
            var stringData = Encoding.Default.GetString(data);
            Execute(stringData, onSuccess, onError);
        }
    }
}