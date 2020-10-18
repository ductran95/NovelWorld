using Microsoft.EntityFrameworkCore;
using NovelWorld.Data.Enums;

namespace NovelWorld.Infrastructure.EntityFramework.Extensions
{
    public static class EntityStateEnumExtensions
    {
        public static EntityStateEnum GetState(this EntityState state, bool isDeleted)
        {
            switch (state)
            {
                case EntityState.Added:
                    return EntityStateEnum.Add;
                
                case EntityState.Modified:
                    return isDeleted ? EntityStateEnum.Delete : EntityStateEnum.Update;
                
                case EntityState.Deleted:
                    return EntityStateEnum.Delete;
                
                default:
                    return EntityStateEnum.UnChange;
            }
        }
    }
}