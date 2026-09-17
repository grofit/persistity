using System;
namespace Persistity.Core.Data
{
    public struct DataObject
    {
        private readonly string _stringData;
        private readonly byte[] _byteData;

        public string AsString => _stringData ?? DefaultEncoding.Encoder.GetString(_byteData);
        public byte[] AsBytes
        {
            get
            {
                if (_byteData != null) { return _byteData; }
                if (string.IsNullOrEmpty(_stringData)) { return new byte[0]; }
                return DefaultEncoding.Encoder.GetBytes(_stringData);
            }
        }

        public DataObject(string data)
        {
            _stringData = data;
            _byteData = null;
        }

        public DataObject(byte[] data)
        {
            _stringData = null;
            _byteData = data;
        }
        
    }
}