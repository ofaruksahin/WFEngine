using System.Threading.Tasks;
using WFEngineCore.Models;

namespace WFEngineCore.Interface
{
    public interface IStepBody
    {        
        Task<ExecutionResult> RunAsync(IStepExecutionContext context);        
    }
}
