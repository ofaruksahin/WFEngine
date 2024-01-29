using System.Reflection;
using WFEngine.Domain.Common.Contracts;

namespace WFEngine.Application.Common.Options
{
    public class MapperOptions : IOptions
    {
        public string Key => "WFEngine:Options:Mapper";
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

        public MapperOptions()
        {
            AssemblyNames = new string[0];
        }
    }
}
