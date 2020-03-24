using System;
using System.Threading.Tasks;
using LazyData;
using Persistity.Pipelines.Steps.Types;

namespace Persistity.Pipelines.Steps
{
    public class SendDataToObjectMethodStep : IPipelineStep, IExpectsData, IReturnsObject
    {
        private readonly Func<DataObject, object, Task<object>> _methodWithState;
        private readonly Func<DataObject, Task<object>> _methodWithoutState;

        public SendDataToObjectMethodStep(Func<DataObject, Task<object>> method)
        { _methodWithoutState = method; }
        
        public SendDataToObjectMethodStep(Func<DataObject, object, Task<object>> methodWithState)
        { _methodWithState = methodWithState; }
        
        public Task<object> Execute(object data, object state = null)
        {
            return _methodWithState == null ? 
                _methodWithoutState((DataObject)data) : 
                _methodWithState((DataObject)data, state);
        }
    }
}