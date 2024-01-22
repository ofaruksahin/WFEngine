﻿using System;
using System.Collections.Generic;
using WFEngineCore.Attributes;
using WFEngineCore.Exceptions;
using WFEngineCore.Interface;
using WFEngineCore.Models;

namespace WFEngineCore.Primitives
{
    [IgnoreActivity]
    [Obsolete]
    public class If : ContainerStepBody
    {
        public bool Condition { get; set; }                

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            if (context.PersistenceData == null)
            {
                if (Condition)
                {
                    return ExecutionResult.Branch(new List<object> { context.Item }, new ControlPersistenceData { ChildrenActive = true });
                }
                
                return ExecutionResult.Next();
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
    }
}
