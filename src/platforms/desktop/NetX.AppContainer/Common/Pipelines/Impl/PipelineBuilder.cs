using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppContainer
{
    public class PipelineBuilder<TResult> : IPipelineBuilder<TResult>
    {
        private readonly List<IPipelineStep<TResult>> _steps = new List<IPipelineStep<TResult>>();
        private IDataSharingPipelineContext<TResult> _context;

        public PipelineBuilder(IDataSharingPipelineContext<TResult> context)
        {
            _context = context;
        }

        public IPipelineBuilder<TResult> Register(IPipelineStep<TResult> step)
        {
            _steps.Add(step);
            return this;
        }

        public IPipeline<TResult> Build()
        {
            return new Pipeline<TResult>(_context, _steps.ToArray());
        }
    }
}
