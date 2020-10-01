using System;

namespace NovelWorld.Data.Entities
{
    public class Entity
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime DeletedOn { get; set; }
        public Guid DeletedBy { get; set; }
    }
}