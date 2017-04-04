using Persistity.Attributes;

namespace Tests.Editor.Models
{
    [Persist]
    public class E
    {
        [PersistData]
        public int IntValue { get; set; }
    }
}