using System;
using WFEngineCore.Attributes;
using WFEngineCore.Interface;
using WFEngineCore.Models;

namespace WFEngineCore.Primitives
{
    [IgnoreActivity]
    [Obsolete]
    public class WorkflowStepInline : WorkflowStep<InlineStepBody>
    {
        public Func<IStepExecutionContext, ExecutionResult> Body { get; set; }

        public override IStepBody ConstructBody(IServiceProvider serviceProvider)
        {
            return new InlineStepBody(Body);
        }
    }
}
