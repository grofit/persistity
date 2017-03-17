using Persistity.Attributes;

namespace Tests.Editor.Models
{
    [Persist]
    public class C
    {
        [PersistData]
        public float FloatValue { get; set; }
    }
}