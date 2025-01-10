namespace Persistity.Core.Data
{
    public struct DataObject
    {
        private readonly string _stringData;
        private readonly byte[] _byteData;

        public string AsString => _stringData ?? DefaultEncoding.Encoder.GetString(_byteData);
        public byte[] AsBytes => _byteData ?? DefaultEncoding.Encoder.GetBytes(_stringData);

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