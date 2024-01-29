using Microsoft.Extensions.DependencyInjection;

namespace WFEngine.Infrastructure.Common.IoC.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class InjectAttribute : Attribute
    {
        public ServiceLifetime Lifetime { get; private set; }

        public InjectAttribute(ServiceLifetime lifetime)
        {
            Lifetime = lifetime;
        }
    }
}
