using System;
using System.Collections.Generic;
using WFEngineCore.Models;

namespace WFEngineCore.Interface
{
    public interface IWorkflowErrorHandler
    {
        WorkflowErrorHandling Type { get; }
        void Handle(WorkflowInstance workflow, WorkflowDefinition def, ExecutionPointer pointer, WorkflowStep step, Exception exception, Queue<ExecutionPointer> bubbleUpQueue);
    }
}
