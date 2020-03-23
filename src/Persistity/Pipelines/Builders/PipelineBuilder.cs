using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistity.Endpoints;
using Persistity.Pipelines.Steps;
using Persistity.Pipelines.Steps.Types;

namespace Persistity.Pipelines.Builders
{
    public class PipelineBuilder
    {
        public PipelineNeedsObjectBuilder StartFromInput()
        { return new PipelineNeedsObjectBuilder(new List<IPipelineStep>()); }
        
        public PipelineNeedsObjectBuilder StartFrom(Func<Task<object>> method)
        { return new PipelineNeedsObjectBuilder(new List<IPipelineStep>{ new ReceiveMethodStep(method)}); }
        
        public PipelineNeedsObjectBuilder StartFrom(Func<object, Task<object>> method)
        { return new PipelineNeedsObjectBuilder(new List<IPipelineStep>{ new ReceiveMethodStep(method)}); }
        
        public PipelineNeedsDataBuilder StartFrom(IReceiveDataEndpoint endpoint)
        { return new PipelineNeedsDataBuilder(new List<IPipelineStep>{ new ReceiveEndpointStep(endpoint)}); }

        public PipelineNeedsObjectBuilder ForkObjectFrom(IFlowPipeline flowPipeline, int forkAtStep = -1)
        {
            var stepsToTake = forkAtStep == -1 ? 
                flowPipeline.Steps.ToList() : 
                flowPipeline.Steps.Take(forkAtStep).ToList();
            
            if(stepsToTake.Last() is IReturnsData)
            { throw new ArgumentException("Step being forked returns Data not an Object", nameof(forkAtStep)); }
            
            return new PipelineNeedsObjectBuilder(stepsToTake);
        }
        
        public PipelineNeedsDataBuilder ForkDataFrom(IFlowPipeline flowPipeline, int forkAtStep = -1)
        {
            var stepsToTake = forkAtStep == -1 ? 
                flowPipeline.Steps.ToList() : 
                flowPipeline.Steps.Take(forkAtStep).ToList();
            
            if(stepsToTake.Last() is IReturnsObject)
            { throw new ArgumentException("Step being forked returns an Object not Data", nameof(forkAtStep)); }
            
            return new PipelineNeedsDataBuilder(stepsToTake);
        }
    }
}