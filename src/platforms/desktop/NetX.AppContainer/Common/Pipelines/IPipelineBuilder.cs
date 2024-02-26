using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppContainer
{
    public interface IPipelineBuilder<TResult>
    {
        IPipelineBuilder<TResult> Register(IPipelineStep<TResult> step);
        IPipeline<TResult> Build();
    }
}
