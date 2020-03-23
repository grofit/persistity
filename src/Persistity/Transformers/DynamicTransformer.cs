using System;

namespace Persistity.Transformers
{
    public class DynamicTransformer : ITransformer
    {
        private readonly Func<object, object> _transformMethod;

        public DynamicTransformer(Func<object, object> transformMethod)
        { _transformMethod = transformMethod; }

        public object Transform(object original)
        { return _transformMethod(original); }
    }
}