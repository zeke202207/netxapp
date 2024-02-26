using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppContainer
{
    public interface IDataSharingPipelineContext<TResult> : IPipelineContext<TResult>
    {
        IDictionary<string, object> SharedData { get; }
        event EventHandler<StepExecutionArgs> StepExecuted;
    }
}
