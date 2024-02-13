namespace WFEngine.Application.Common.Pipelines.Validation
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public  class ValidateAttribute : Attribute
    {
        public Type ValidatorType { get; private set; }
        public Type ValidatorContextType { get; set; }

        public ValidateAttribute(Type validatorType,Type validatorContextType)
        {
            ValidatorType = validatorType;
            ValidatorContextType = validatorContextType;
        }
    }
}
