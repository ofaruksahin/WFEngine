using System;
using WFEngineCore.Attributes;
using WFEngineCore.Models;

namespace WFEngineCore.Primitives
{
    [Activity("End", "This step is used to terminate your current workflow","Basic")]
    public class EndStep : WorkflowStep
    {
        public override Type BodyType => null;

        public override ExecutionPipelineDirective InitForExecution(
            WorkflowExecutorResult executorResult, 
            WorkflowDefinition defintion, 
            WorkflowInstance workflow, 
            ExecutionPointer executionPointer)
        {
            return ExecutionPipelineDirective.EndWorkflow;
        }
    }
}
