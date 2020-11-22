using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NovelWorld.Authentication.Contexts.Implements;
using NovelWorld.Data.Entities;
using NovelWorld.Data.Enums;
using NovelWorld.Infrastructure.EntityFrameworkCore.Extensions;
using NovelWorld.Infrastructure.EventSourcing;
using NovelWorld.Infrastructure.EventSourcing.Abstractions;

namespace NovelWorld.Infrastructure.EntityFrameworkCore.Contexts
{
    public class EfCoreEntityContext: DbContext
    {
        private readonly IAuthContext _authContext;
        private readonly IDbEventSource _dbEventSource;
        
        public EfCoreEntityContext([NotNull] DbContextOptions options, IAuthContext authContext, IDbEventSource dbEventSource): base(options)
        {
            _authContext = authContext;
            _dbEventSource = dbEventSource;
            
            // ReSharper disable once VirtualMemberCallInConstructor
            ChangeTracker.Tracked += OnEntityTracked;
            // ReSharper disable once VirtualMemberCallInConstructor
            ChangeTracker.StateChanged += OnEntityStateChanged;
        }

        void OnEntityTracked(object sender, EntityTrackedEventArgs e)
        {
            if (!e.FromQuery && e.Entry.Entity is Entity entity)
            {
                var userId = _authContext.User.Id;
                entity.State = e.Entry.State.GetState(entity.IsDeleted);
                entity.SetContext(userId);

                CreateDbChangedEvent(entity, userId);
            }
        }

        void OnEntityStateChanged(object sender, EntityStateChangedEventArgs e)
        {
            if (e.Entry.Entity is Entity entity)
            {
                var userId = _authContext.User.Id;
                entity.State = e.Entry.State.GetState(entity.IsDeleted);
                entity.SetContext(userId);

                CreateDbChangedEvent(entity, userId);
            }
        }

        // public override int SaveChanges(bool acceptAllChangesOnSuccess)
        // {
        //     SetChangedEntitiesContext();
        //     return base.SaveChanges(acceptAllChangesOnSuccess);
        // }
        
        // public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        // {
        //     SetChangedEntitiesContext();
        //     return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        // }

        public void CreateDbChangedEvent(Entity entity, Guid userId)
        {
            _dbEventSource.Add(new DbChangedEvent
            {
                UserId = userId,
                Name = entity.GetType().Name,
                Action = entity.State.ToString(),
                Data = entity.State != EntityStateEnum.Delete ? entity : (object) entity.Id
            });
        } 

        // public virtual void SetChangedEntitiesContext()
        // {
        //     var changedObject = ChangeTracker.Entries();
        //     var userId = _authContext.User.Id;
        //     foreach (var entry in changedObject)
        //     {
        //         if (entry.Entity is Entity entity)
        //         {
        //             entity.State = entry.State.GetState(entity.IsDeleted);
        //             entity.SetContext(userId);
        //
        //             CreateDbChangedEvent(entity, userId);
        //         }
        //     }
        // }
    }
}