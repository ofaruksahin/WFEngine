using System;
using WFEngineCore.Attributes;
using WFEngineCore.Interface;
using WFEngineCore.Models;

namespace WFEngineCore.Primitives
{
    [IgnoreActivity]
    [Obsolete]
    public class Delay : StepBody
    {
        public TimeSpan Period { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            if (context.PersistenceData != null)
            {
                return ExecutionResult.Next();
            }
            
            return ExecutionResult.Sleep(Period, true);
        }
    }
}
