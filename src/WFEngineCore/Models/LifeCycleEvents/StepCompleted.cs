using System;

namespace WFEngineCore.Models.LifeCycleEvents
{
    public class StepCompleted : LifeCycleEvent
    {
        public string ExecutionPointerId { get; set; }

        public int StepId { get; set; }
    }
}
