using System;

namespace WFEngineCore.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ActivityAttribute : Attribute
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string CategoryName { get; private set; }

        public ActivityAttribute(
            string name,
            string description, 
            string categoryName)
        {
            Name = name;
            Description = description;
            CategoryName = categoryName;
        }
    }
}
