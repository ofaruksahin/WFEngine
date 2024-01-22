using System;

namespace WFEngineCore.Exceptions
{
    public class WorkflowDefinitionLoadException : Exception
    {
        public WorkflowDefinitionLoadException(string message)
            : base (message)
        {            
        }
    }
}
