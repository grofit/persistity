using Persistity.Attributes;
using Tests.Editor.Models;

namespace Assets.Tests.Editor
{
    [Persist]
    public class B
    {
        [PersistData]
        public string StringValue { get; set; }

        [PersistData]
        public int  IntValue { get; set; }

        [PersistData]
        public C[] NestedArray { get; set; }
    }
}