using Persistity.Attributes;

namespace Tests.Editor.Models
{
    public class C
    {
        [PersistData]
        public float FloatValue { get; set; }
    }
}