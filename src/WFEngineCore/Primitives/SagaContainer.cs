using System;
using WFEngineCore.Attributes;
using WFEngineCore.Interface;
using WFEngineCore.Models;

namespace WFEngineCore.Primitives
{
    [IgnoreActivity]
    [Obsolete]
    public class SagaContainer<TStepBody> : WorkflowStep<TStepBody>
        where TStepBody : IStepBody
    {
        public override bool ResumeChildrenAfterCompensation => false;
        public override bool RevertChildrenAfterCompensation => true;

        public override void PrimeForRetry(ExecutionPointer pointer)
        {
            base.PrimeForRetry(pointer);
            pointer.PersistenceData = null;
        }
    }
}
