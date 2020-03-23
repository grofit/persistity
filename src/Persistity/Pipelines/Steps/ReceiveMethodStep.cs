using System;
using System.Threading.Tasks;
using Persistity.Pipelines.Steps.Types;

namespace Persistity.Pipelines.Steps
{
    public class ReceiveMethodStep : IPipelineStep, IReturnsObject
    {
        private readonly Func<Task<object>> _methodWithoutState;
        private readonly Func<object, Task<object>> _methodWithState;

        public ReceiveMethodStep(Func<Task<object>> methodWithoutState)
        { _methodWithoutState = methodWithoutState; }

        public ReceiveMethodStep(Func<object, Task<object>> methodWithState)
        { _methodWithState = methodWithState; }

        public Task<object> Execute(object data = null, object state = null)
        {
            return _methodWithState == null ? 
                _methodWithoutState() : 
                _methodWithState(state);
        }
    }
}