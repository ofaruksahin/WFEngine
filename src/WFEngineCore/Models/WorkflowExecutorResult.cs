using System.Collections.Generic;

namespace WFEngineCore.Models
{
    public class WorkflowExecutorResult
    {
        public List<EventSubscription> Subscriptions { get; set; } = new List<EventSubscription>();
        public List<ExecutionError> Errors { get; set; } = new List<ExecutionError>();
    }
}
