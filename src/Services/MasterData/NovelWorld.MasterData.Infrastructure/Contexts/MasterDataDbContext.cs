using Microsoft.EntityFrameworkCore;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Authentication.Contexts.Implements;
using NovelWorld.Infrastructure.EntityFrameworkCore.Contexts;
using NovelWorld.Infrastructure.EventSourcing.Abstractions;

namespace NovelWorld.MasterData.Infrastructure.Contexts
{
    public class MasterDataDbContext : EfCoreEntityDbContext
    {
        public MasterDataDbContext(
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