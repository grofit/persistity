using System;
using System.Threading.Tasks;
using Persistity.Flow.Steps.Types;

namespace Persistity.Flow.Steps
{
    public class SendMethodStep : IPipelineStep, IExpectsObject, IReturnsObject
    {
        private readonly Func<object, object, Task<object>> _methodWithState;
        private readonly Func<object, Task<object>> _methodWithoutState;

        public SendMethodStep(Func<object, Task<object>> method)
        { _methodWithoutState = method; }
        
        public SendMethodStep(Func<object, object, Task<object>> methodWithState)
        { _methodWithState = methodWithState; }
        
        public Task<object> Execute(object data, object state = null)
        {
            return _methodWithState == null ? 
                _methodWithoutState(data) : 
                _methodWithState(data, state);
        }
    }
}