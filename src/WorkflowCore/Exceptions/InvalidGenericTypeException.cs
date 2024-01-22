using System;

namespace WorkflowCore.Exceptions
{
    public class InvalidGenericTypeException : Exception
    {
        public InvalidGenericTypeException(string typeName) : base($"{typeName} is not valid type")
        {
            
        }
    }
}
