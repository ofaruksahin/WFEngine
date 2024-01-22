using System.Threading;
using System.Threading.Tasks;
using WFEngineCore.Models;

namespace WFEngineCore.Interface
{
    public interface IWorkflowExecutor
    {
        Task<WorkflowExecutorResult> Execute(WorkflowInstance workflow, CancellationToken cancellationToken = default);
    }
}