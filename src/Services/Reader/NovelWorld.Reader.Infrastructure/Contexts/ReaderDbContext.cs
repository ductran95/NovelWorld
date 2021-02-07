using Microsoft.EntityFrameworkCore;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Infrastructure.EntityFrameworkCore.Contexts;
using NovelWorld.Infrastructure.EventSourcing.Abstractions;
using NovelWorld.Reader.Data.Entities;
using NovelWorld.Reader.Infrastructure.Configurations;

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

        public DbSet<BookComment> BookComments { get; set; }
        public DbSet<BookReview> BookReviews { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<ReadingHistory> ReadingHistories { get; set; }
        public DbSet<SubscribedBook> SubscribedBooks { get; set; }
        
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfiguration(new BookCommentConfiguration());
            modelBuilder.ApplyConfiguration(new BookReviewConfiguration());
            modelBuilder.ApplyConfiguration(new BookmarkConfiguration());
            modelBuilder.ApplyConfiguration(new ReadingHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new SubscribedBookConfiguration());
        }
    }
}