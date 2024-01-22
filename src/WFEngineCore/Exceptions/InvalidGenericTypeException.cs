using System;

namespace WFEngineCore.Exceptions
{
    public class InvalidGenericTypeException : Exception
    {
        public InvalidGenericTypeException(string typeName) : base($"{typeName} is not valid type")
        {
            
        }
    }
}
