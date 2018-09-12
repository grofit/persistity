using System.Threading.Tasks;
using LazyData;
using Persistity.Encryption;

namespace Persistity.Processors.Encryption
{
    public class DecryptDataProcessor : IProcessor
    {
        public IEncryptor Encryptor { get; private set; }

        public DecryptDataProcessor(IEncryptor encryptor)
        { Encryptor = encryptor; }

        public Task<DataObject> Process(DataObject data)
        {
            var decryptedData = Encryptor.Decrypt(data.AsBytes);
            return Task.FromResult(new DataObject(decryptedData));
        }
    }
}