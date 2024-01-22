﻿using System;

namespace WFEngineCore.Exceptions
{
    public class WorkflowNotRegisteredException : Exception
    {
        public WorkflowNotRegisteredException(string workflowId, int? version)
            : base($"Workflow {workflowId} {version} is not registered")
        {
        }
    }
}
