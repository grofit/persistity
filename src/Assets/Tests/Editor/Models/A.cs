using System.Collections.Generic;
using Persistity.Attributes;
using Tests.Editor.Models;

namespace Assets.Tests.Editor
{
    [Persist]
    public class A
    {
        [PersistData]
        public string TestValue { get; set; }

        public int NonPersisted { get; set; }

        [PersistData]
        public B NestedValue { get; set; }

        [PersistData]
        public B[] NestedArray { get; set; }
        
        [PersistData]
        public IList<string> Stuff { get; set; }

        [PersistData]
        public D AllTypes { get; set; }

        public A()
        {
            Stuff = new List<string>();
        }
    }
}
