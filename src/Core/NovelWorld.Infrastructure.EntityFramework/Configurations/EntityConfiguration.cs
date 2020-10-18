using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovelWorld.Data.Entities;

namespace NovelWorld.Infrastructure.EntityFramework.Configurations
{
    public class EntityConfiguration<T>: IEntityTypeConfiguration<T> where T: Entity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Ignore(x => x.State);
            builder.HasQueryFilter(p => !p.IsDeleted);
        }
    }
}