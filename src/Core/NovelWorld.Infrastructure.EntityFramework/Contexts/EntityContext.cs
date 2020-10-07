using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NovelWorld.Authentication.Contexts;
using NovelWorld.Common.Enums;
using NovelWorld.Data.Entities;
using NovelWorld.Infrastructure.EntityFramework.Extensions;
using NovelWorld.Infrastructure.EventSourcing;

namespace NovelWorld.Infrastructure.EntityFramework.Contexts
{
    public abstract class EntityContext: DbContext
    {
        protected readonly IAuthContext AuthContext;
        protected readonly IEventSource _eventSource;
        
        public EntityContext([NotNull] DbContextOptions options, IAuthContext authContext, IEventSource eventSource): base(options)
        {
            AuthContext = authContext;
            _eventSource = eventSource;
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
                    
                    _eventSource.Add(new DbChangedEvent
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