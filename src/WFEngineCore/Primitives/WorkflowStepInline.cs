using System;
using WFEngineCore.Interface;
using WFEngineCore.Models;

namespace WFEngineCore.Primitives
{
    public class WorkflowStepInline : WorkflowStep<InlineStepBody>
    {
        public Func<IStepExecutionContext, ExecutionResult> Body { get; set; }

        public override IStepBody ConstructBody(IServiceProvider serviceProvider)
        {
            return new InlineStepBody(Body);
        }
    }
}
