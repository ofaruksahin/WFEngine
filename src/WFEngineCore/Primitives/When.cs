﻿using System;
using System.Collections.Generic;
using System.Linq;
using WFEngineCore.Attributes;
using WFEngineCore.Exceptions;
using WFEngineCore.Interface;
using WFEngineCore.Models;

namespace WFEngineCore.Primitives
{
    [IgnoreActivity]
    [Obsolete]
    public class When : ContainerStepBody
    {
        public object ExpectedOutcome { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            var switchOutcome = GetSwitchOutcome(context);

            if (ExpectedOutcome != switchOutcome)
            {
                if (Convert.ToString(ExpectedOutcome) != Convert.ToString(switchOutcome))
                {
                    return ExecutionResult.Next();
                }
            }

            if (context.PersistenceData == null)
            {
                return ExecutionResult.Branch(new List<object> { context.Item }, new ControlPersistenceData { ChildrenActive = true });
            }

            if ((context.PersistenceData is ControlPersistenceData) && ((context.PersistenceData as ControlPersistenceData).ChildrenActive))
            {
                if (context.Workflow.IsBranchComplete(context.ExecutionPointer.Id))
                {
                    return ExecutionResult.Next();
                }
                    
                return ExecutionResult.Persist(context.PersistenceData);
            }

            throw new CorruptPersistenceDataException();
        }        

        private object GetSwitchOutcome(IStepExecutionContext context)
        {
            var switchPointer = context.Workflow.ExecutionPointers.First(x => x.Children.Contains(context.ExecutionPointer.Id));
            return switchPointer.Outcome;
        }
    }
}
