using WFEngineCore.Models;

namespace WFEngineCore.Interface
{
    public interface IExecutionPointerFactory
    {
        ExecutionPointer BuildGenesisPointer(WorkflowDefinition def);
        ExecutionPointer BuildCompensationPointer(WorkflowDefinition def, ExecutionPointer pointer, ExecutionPointer exceptionPointer, int compensationStepId);
        ExecutionPointer BuildNextPointer(WorkflowDefinition def, ExecutionPointer pointer, IStepOutcome outcomeTarget);
        ExecutionPointer BuildChildPointer(WorkflowDefinition def, ExecutionPointer pointer, int childDefinitionId, object branch);
    }
}