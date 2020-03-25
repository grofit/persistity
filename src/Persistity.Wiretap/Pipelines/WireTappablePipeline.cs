using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistity.Flow.Steps.Types;
using Persistity.Wiretap.Models;

namespace Persistity.Wiretap.Pipelines
{
    public class WireTappablePipeline : IWireTappablePipeline
    {
        public IEnumerable<IPipelineStep> Steps { get; }
        public IDictionary<int, IList<Action<object, object>>> WireTaps { get; }

        public WireTappablePipeline(IEnumerable<IPipelineStep> steps)
        {
            Steps = steps;
            WireTaps = new Dictionary<int, IList<Action<object, object>>>();
        }

        public async Task<object> Execute(object input = null, object state = null)
        {
            if (!Steps.Any()) { return input; }

            var firstStep = Steps.First();
            if(firstStep is IExpectsObject && input == null)
            { throw new ArgumentNullException(nameof(input), "First step is expecting an object input, none has been provided"); }
            
            var currentData = input;

            var stepCount = 0;
            ProcessWiretapsFor(stepCount, currentData, state);
            foreach (var step in Steps)
            {
                currentData = await step.Execute(currentData, state);
                ProcessWiretapsFor(++stepCount, currentData, state);
            }

            return currentData;
        }

        public WireTapSubscription StartWiretap(int step, Action<object, object> action)
        {
            if(!WireTaps.ContainsKey(step))
            { WireTaps[step] = new List<Action<object, object>>(); }

            var wiretaps = WireTaps[step];
            var alreadyBound = wiretaps.Contains(action);
            if (!alreadyBound)
            { wiretaps.Add(action); }
            return new WireTapSubscription(() => { StopWiretap(step, action); });
        }

        protected void ProcessWiretapsFor(int step, object currentData, object state)
        {
            if (!WireTaps.ContainsKey(step)) { return; }
            var wiretaps = WireTaps[step];
            foreach (var wiretap in wiretaps)
            { wiretap(currentData, state); }
        }

        public void StopWiretap(int step, Action<object, object> action)
        {
            if(!WireTaps.ContainsKey(step)) { return; }

            var wiretaps = WireTaps[step];
            if(!wiretaps.Contains(action)) { return; }

            wiretaps.Remove(action);
        }
    }
}