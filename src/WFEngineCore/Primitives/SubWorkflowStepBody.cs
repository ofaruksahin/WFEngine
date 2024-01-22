using System;
using WFEngineCore.Attributes;
using WFEngineCore.Interface;
using WFEngineCore.Models;

namespace WFEngineCore.Primitives
{
    [IgnoreActivity]
    [Obsolete]
    public class SubWorkflowStepBody : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            // TODO: What is this supposed to do?
            throw new NotImplementedException();
        }
    }
}
