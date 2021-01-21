using Microsoft.EntityFrameworkCore;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Authentication.Contexts.Implements;
using NovelWorld.Identity.Data.Entities;
using NovelWorld.Identity.Infrastructure.Configurations;
using NovelWorld.Infrastructure.EntityFrameworkCore.Contexts;
using NovelWorld.Infrastructure.EventSourcing.Abstractions;

namespace NovelWorld.Identity.Infrastructure.Contexts
{
    public class IdentityDbContext : EfCoreEntityDbContext
    {
        public IdentityDbContext(
            DbContextOptions options, 
            IAuthContext authContext, 
            IDbEventSource dbEventSource
            ) : base(options, authContext, dbEventSource)
        {
        }

        #region DbSets

        public DbSet<User> Users { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}