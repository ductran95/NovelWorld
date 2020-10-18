using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NovelWorld.Authentication.Contexts;
using NovelWorld.Data.Entities;
using NovelWorld.Data.Enums;
using NovelWorld.Infrastructure.EntityFramework.Extensions;
using NovelWorld.Infrastructure.EventSourcing;

namespace NovelWorld.Infrastructure.EntityFramework.Contexts
{
    public abstract class EntityContext: DbContext
    {
        protected readonly IAuthContext AuthContext;
        protected readonly IDbEventSource IdbEventSource;
        
        public EntityContext([NotNull] DbContextOptions options, IAuthContext authContext, IDbEventSource idbEventSource): base(options)
        {
            AuthContext = authContext;
            IdbEventSource = idbEventSource;
        }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            SetContext();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            SetContext();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public virtual void SetContext()
        {
            var changedObject = ChangeTracker.Entries();
            var userId = AuthContext.User.Id;
            foreach (var entry in changedObject)
            {
                if (entry.Entity is Entity entity)
                {
                    entity.State = entry.State.GetState(entity.IsDeleted);
                    entity.SetContext(userId);
                    
                    IdbEventSource.Add(new DbChangedEvent
                    {
                        Name = entity.GetType().Name,
                        Action = entity.State.ToString(),
                        Data = entity.State != EntityStateEnum.Delete ? entity : (object) entity.Id
                    });
                }
            }
        }
    }
}