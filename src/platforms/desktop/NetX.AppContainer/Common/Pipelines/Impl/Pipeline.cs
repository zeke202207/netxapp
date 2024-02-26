using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppContainer
{
    public class Pipeline<TResult> : IPipeline<TResult>
    {
        private readonly IPipelineStep<TResult>[] _steps;
        private readonly IPipelineContext<TResult> _context;

        public Pipeline(IPipelineContext<TResult> context, params IPipelineStep<TResult>[] steps)
        {
            _steps = steps;
            _context = context;
        }

        public async Task<TResult> ExecuteAsync()
        {
            foreach (var step in _steps)
            {
                _context.CancellationToken.ThrowIfCancellationRequested();
                await step.ExecuteAsync(_context);
            }
            return _context.Result;
        }
    }
}
