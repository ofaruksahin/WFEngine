namespace WFEngine.Infrastructure.Common.Data.EntityFrameworkCore.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DbContextEntityTypeConfigurationAttribute : Attribute
    {
        public Type DbContext { get; private set; }

        public DbContextEntityTypeConfigurationAttribute(Type dbContext)
        {
            DbContext = dbContext;
        }
    }
}
