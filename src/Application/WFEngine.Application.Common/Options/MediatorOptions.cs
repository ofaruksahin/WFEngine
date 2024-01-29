using System.Reflection;
using WFEngine.Domain.Common.Contracts;

namespace WFEngine.Application.Common.Options
{
    public class MediatorOptions : IOptions
    {
        public string Key => "WFEngine:Options:Mediator";
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

        public MediatorOptions()
        {
            AssemblyNames = new string[0];
        }
    }
}
