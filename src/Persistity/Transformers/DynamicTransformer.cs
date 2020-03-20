using System;

namespace Persistity.Transformers
{
    public class DynamicTransformer : ITransformer
    {
        private Func<object, object> _to;
        private Func<object, object> _from;

        public DynamicTransformer(Func<object, object> to = null, Func<object, object> from = null)
        {
            _to = to;
            _from = from;
        }

        public object TransformTo(object original)
        {
            if(_to != null) { return _to(original); }
            throw new System.NotImplementedException();
        }

        public object TransformFrom(object converted)
        {
            if(_from != null) { return _from(converted); }
            throw new System.NotImplementedException();
        }
    }
}