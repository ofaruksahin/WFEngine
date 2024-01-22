using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace WFEngine.Infrastructure.Common.Data.EntityFrameworkCore
{
    public class BaseDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
