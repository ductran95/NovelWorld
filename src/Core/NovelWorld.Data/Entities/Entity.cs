using System;
using NovelWorld.Data.Enums;

namespace NovelWorld.Data.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public Guid? DeletedBy { get; set; }
        
        public EntityStateEnum State { get; set; }

        public void SetContext(Guid userId)
        {
            var time = DateTime.UtcNow;
            ModifiedBy = userId;
            ModifiedOn = time;

            if (State == EntityStateEnum.Add)
            {
                CreatedBy = userId;
                CreatedOn = time;
            }

            if (State == EntityStateEnum.Delete)
            {
                DeletedBy = userId;
                DeletedOn = time;
            }
        }
    }
}