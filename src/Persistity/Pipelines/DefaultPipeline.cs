using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistity.Pipelines.Steps.Types;

namespace Persistity.Pipelines
{
    public class DefaultPipeline : IPipeline
    {
        private readonly IEnumerable<IPipelineStep> _steps;

        public DefaultPipeline(IEnumerable<IPipelineStep> steps)
        { _steps = steps; }
        
        public DefaultPipeline(params IPipelineStep[] steps)
        { _steps = steps; }

        public async Task<object> Execute(object input = null, object state = null)
        {
            if (!_steps.Any()) { return input; }

            var firstStep = _steps.First();
            if(firstStep is IExpectsObject && input == null)
            { throw new ArgumentNullException(nameof(input), "First step is expecting an object input, none has been provided"); }
            
            var currentData = input;
            
            foreach (var step in _steps)
            { currentData = await step.Execute(currentData, state); }

            return currentData;
        }
    }
}