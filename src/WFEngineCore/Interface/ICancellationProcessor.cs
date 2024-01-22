using System;
using WFEngineCore.Models;

namespace WFEngineCore.Interface
{
    public interface ICancellationProcessor
    {
        void ProcessCancellations(WorkflowInstance workflow, WorkflowDefinition workflowDef, WorkflowExecutorResult executionResult);
    }
}
