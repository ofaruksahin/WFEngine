using System;
using WFEngineCore.Attributes;
using WFEngineCore.Interface;
using WFEngineCore.Models;

namespace WFEngineCore.Primitives
{
    [IgnoreActivity]
    [Obsolete]
    public class ActionStepBody : StepBody
    {
        public Action<IStepExecutionContext> Body { get; set; }
        
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Body(context);
            return ExecutionResult.Next();
        }
    }
}
