using System;
using WFEngineCore.Attributes;
using WFEngineCore.Interface;
using WFEngineCore.Models;

namespace WFEngineCore.Primitives
{
    [IgnoreActivity]
    [Obsolete]
    public class Decide : StepBody
    {
        public object Expression { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            return ExecutionResult.Outcome(Expression);
        }
    }
}
