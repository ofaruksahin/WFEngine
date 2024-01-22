using System;
using System.Threading.Tasks;
using WFEngineCore.Interface;

namespace WFEngineCore.Models
{
    public abstract class StepBody : IStepBody
    {
        public abstract ExecutionResult Run(IStepExecutionContext context);

        public Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            return Task.FromResult(Run(context));
        }        

        protected ExecutionResult OutcomeResult(object value)
        {
            return new ExecutionResult
            {
                Proceed = true,
                OutcomeValue = value
            };
        }

        protected ExecutionResult PersistResult(object persistenceData)
        {
            return new ExecutionResult
            {
                Proceed = false,
                PersistenceData = persistenceData
            };
        }

        protected ExecutionResult SleepResult(object persistenceData, TimeSpan sleep)
        {
            return new ExecutionResult
            {
                Proceed = false,
                PersistenceData = persistenceData,
                SleepFor = sleep
            };
        }
    }

    public abstract class StepBody<T1> : StepBody
    {
    }

    public abstract class StepBody<T1,T2> : StepBody
    {
    }

    public abstract class StepBody<T1,T2,T3> : StepBody
    {
    }
}
