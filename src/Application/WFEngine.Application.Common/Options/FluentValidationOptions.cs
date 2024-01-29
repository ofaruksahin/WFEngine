using System.Reflection;
using WFEngine.Domain.Common.Contracts;

namespace WFEngine.Application.Common.Options
{
    public class FluentValidationOptions : IOptions
    {
        public string Key => "WFEngine:Options:Validator";
        public string[] AssemblyNames { get; set; }
        public Assembly[] Assemblies
        {
            get
            {
                return AssemblyNames
                    .Select(assemblyName => Assembly.Load(assemblyName))
                    .ToArray();
            }
        }

        public FluentValidationOptions()
        {
            AssemblyNames = new string[0];
        }
    }
}
