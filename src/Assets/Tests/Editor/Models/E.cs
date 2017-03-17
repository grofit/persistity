using Persistity.Attributes;

namespace Tests.Editor.Models
{
    [Persist]
    public class E
    {
        [PersistData]
        public float IntValue { get; set; }
    }
}