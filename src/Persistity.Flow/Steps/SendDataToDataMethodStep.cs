using System;
using System.Threading.Tasks;
using Persistity.Core;
using Persistity.Flow.Steps.Types;

namespace Persistity.Flow.Steps
{
    public class SendDataToDataMethodStep : IPipelineStep, IExpectsData, IReturnsObject
    {
        private readonly Func<DataObject, object, Task<DataObject>> _methodWithState;
        private readonly Func<DataObject, Task<DataObject>> _methodWithoutState;

        public SendDataToDataMethodStep(Func<DataObject, Task<DataObject>> method)
        { _methodWithoutState = method; }
        
        public SendDataToDataMethodStep(Func<DataObject, object, Task<DataObject>> methodWithState)
        { _methodWithState = methodWithState; }
        
        public async Task<object> Execute(object data, object state = null)
        {
            return _methodWithState == null ?
                await _methodWithoutState((DataObject)data) : 
                await _methodWithState((DataObject)data, state);
        }
    }
}