using System;
using System.Threading;
using System.Threading.Tasks;
using WFEngineCore.Models;

namespace WFEngineCore.Interface
{
    public interface IWorkflowPurger
    {
        Task PurgeWorkflows(WorkflowStatus status, DateTime olderThan, CancellationToken cancellationToken = default);
    }
}