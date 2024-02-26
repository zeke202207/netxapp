using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppContainer
{
    public class StepExecutionArgs : EventArgs
    {
        public string StepName { get; private set; }
        public bool IsSuccess { get; private set; }
        public Exception Error { get; private set; }

        public StepExecutionArgs(string stepName, bool isSuccess, Exception error = null)
        {
            StepName = stepName;
            IsSuccess = isSuccess;
            Error = error;
        }
    }
}
