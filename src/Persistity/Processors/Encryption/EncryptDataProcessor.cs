using System.Threading.Tasks;
using Persistity.Core;
using Persistity.Encryption;

namespace Persistity.Processors.Encryption
{
    public class EncryptDataProcessor : IProcessor
    {
        public IEncryptor Encryptor { get; }

        public EncryptDataProcessor(IEncryptor encryptor)
        { Encryptor = encryptor; }

        public Task<DataObject> Process(DataObject data)
        {
            var encryptedData = Encryptor.Encrypt(data.AsBytes);
            return Task.FromResult(new DataObject(encryptedData));
        }
    }
}