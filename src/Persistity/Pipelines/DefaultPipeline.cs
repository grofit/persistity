using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistity.Pipelines.Steps.Types;

namespace Persistity.Pipelines
{
    public class DefaultPipeline : IPipeline, IFlowPipeline
    {
        public IEnumerable<IPipelineStep> Steps { get; }

        public DefaultPipeline(IEnumerable<IPipelineStep> steps)
        { Steps = steps; }
        
        public DefaultPipeline(params IPipelineStep[] steps)
        { Steps = steps; }

        public async Task<object> Execute(object input = null, object state = null)
        {
            if (!Steps.Any()) { return input; }

            var firstStep = Steps.First();
            if(firstStep is IExpectsObject && input == null)
            { throw new ArgumentNullException(nameof(input), "First step is expecting an object input, none has been provided"); }
            
            var currentData = input;
            
            foreach (var step in Steps)
            { currentData = await step.Execute(currentData, state); }

            return currentData;
        }
    }
}