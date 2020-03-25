using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistity.Flow.Steps.Types;

namespace Persistity.Flow.Pipelines
{
    public abstract class FlowPipeline : IFlowPipeline
    {
        public IEnumerable<IPipelineStep> Steps { get; protected set; }

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