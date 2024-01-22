using System.Threading.Tasks;
using WorkflowCore.Interface;

namespace WorkflowCore.Models
{
    public abstract class StepBodyAsync : IStepBody
    {
        public abstract Task<ExecutionResult> RunAsync(IStepExecutionContext context);
    }

    public abstract class StepBodyAsync<T1> : StepBodyAsync
    {
    }

    public abstract class StepBodyAsync<T1,T2> : StepBodyAsync
    {
    }

    public abstract class StepBodyAsync<T1,T2,T3> : StepBodyAsync
    {
    }
}
