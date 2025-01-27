﻿using System;
using WFEngineCore.Attributes;
using WFEngineCore.Exceptions;
using WFEngineCore.Interface;
using WFEngineCore.Models;

namespace WFEngineCore.Primitives
{
    [IgnoreActivity]
    [Obsolete]
    public class Activity : StepBody
    {
        public string ActivityName { get; set; }
        
        public DateTime EffectiveDate { get; set; }

        public object Parameters { get; set; }
        
        public object Result { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            if (!context.ExecutionPointer.EventPublished)
            {
                DateTime effectiveDate = DateTime.MinValue;

                if (EffectiveDate != null)
                {
                    effectiveDate = EffectiveDate;
                }

                return ExecutionResult.WaitForActivity(ActivityName, Parameters, effectiveDate);
            }

            if (context.ExecutionPointer.EventData is ActivityResult)
            {
                var actResult = (context.ExecutionPointer.EventData as ActivityResult);
                if (actResult.Status == ActivityResult.StatusType.Success)
                {
                    Result = actResult.Data;
                }
                else
                {
                    throw new ActivityFailedException(actResult.Data);
                }
            }
            else
            {
                Result = context.ExecutionPointer.EventData;
            }

            return ExecutionResult.Next();
        }
    }
}
