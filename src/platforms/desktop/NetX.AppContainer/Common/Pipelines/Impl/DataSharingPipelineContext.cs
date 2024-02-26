using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetX.AppContainer
{
    public class DataSharingPipelineContext<TResult> : IDataSharingPipelineContext<TResult>
    {
        public CancellationToken CancellationToken { get; set; }

        public TResult Result { get; set; }

        public IDictionary<string, object> SharedData { get; private set; } = new Dictionary<string, object>();

        public event EventHandler<StepExecutionArgs> StepExecuted;

        public void OnStepExecuted(object sender, string stepName, bool isSuccess, Exception error = null)
        {
            StepExecuted?.Invoke(sender, new StepExecutionArgs(stepName, isSuccess, error));
        }
    }
}
