using Microsoft.EntityFrameworkCore;
using NovelWorld.Authentication.Contexts.Implements;
using NovelWorld.Infrastructure.EntityFrameworkCore.Contexts;
using NovelWorld.Infrastructure.EventSourcing.Abstractions;
using NovelWorld.MasterData.Data.DTO;
using NovelWorld.MasterData.Infrastructure.Configurations;

namespace NovelWorld.MasterData.Infrastructure.Contexts
{
    public class MasterDataContext : EfCoreEntityContext
    {
        public MasterDataContext(
            DbContextOptions options, 
            IAuthContext authContext, 
            IDbEventSource dbEventSource
            ) : base(options, authContext, dbEventSource)
        {
        }

        #region DbSets

        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfiguration(new AuthorConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new ChapterConfiguration());
        }
    }
}