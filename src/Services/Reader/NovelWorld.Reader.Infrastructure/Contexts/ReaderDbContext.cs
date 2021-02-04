using Microsoft.EntityFrameworkCore;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Infrastructure.EntityFrameworkCore.Contexts;
using NovelWorld.Infrastructure.EventSourcing.Abstractions;

namespace NovelWorld.Reader.Infrastructure.Contexts
{
    public class ReaderDbContext : EfCoreEntityDbContext
    {
        public ReaderDbContext(
            DbContextOptions options, 
            IAuthContext authContext, 
            IDbEventSource dbEventSource
            ) : base(options, authContext, dbEventSource)
        {
        }

        #region DbSets

        
        
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            
        }
    }
}