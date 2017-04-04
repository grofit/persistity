using System.Collections.Generic;
using Persistity.Attributes;

namespace Tests.Editor.Models
{
    [Persist]
    public class DynamicTypesModel
    {
        [PersistData]
        public object DynamicNestedProperty { get; set; }

        [PersistData]
        public object DynamicPrimitiveProperty { get; set; }

        [PersistData]
        public IList<object> DynamicList { get; set; }

        [PersistData]
        public IDictionary<object, object> DynamicDictionary { get; set; }
    }
}