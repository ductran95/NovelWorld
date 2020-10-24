using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NovelWorld.Authentication.Contexts;
using NovelWorld.Data.Entities;
using NovelWorld.Data.Enums;
using NovelWorld.Infrastructure.EntityFramework.Extensions;
using NovelWorld.Infrastructure.EventSourcing;
using NovelWorld.Infrastructure.EventSourcing.Abstractions;

namespace NovelWorld.Infrastructure.EntityFramework.Contexts
{
    public abstract class EntityContext: DbContext
    {
        protected readonly IAuthContext _authContext;
        protected readonly IDbEventSource _dbEventSource;
        
        public EntityContext([NotNull] DbContextOptions options, IAuthContext authContext, IDbEventSource dbEventSource): base(options)
        {
            _authContext = authContext;
            _dbEventSource = dbEventSource;
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

        protected virtual void CreateDbChangedEvent(Entity entity, Guid userId)
        {
            _dbEventSource.Add(new DbChangedEvent
            {
                UserId = userId,
                Name = entity.GetType().Name,
                Action = entity.State.ToString(),
                Data = entity.State != EntityStateEnum.Delete ? entity : (object) entity.Id
            });
        } 

        protected virtual void SetContext()
        {
            var changedObject = ChangeTracker.Entries();
            var userId = _authContext.User.Id;
            foreach (var entry in changedObject)
            {
                if (entry.Entity is Entity entity)
                {
                    entity.State = entry.State.GetState(entity.IsDeleted);
                    entity.SetContext(userId);

                    CreateDbChangedEvent(entity, userId);
                }
            }
        }
    }
}