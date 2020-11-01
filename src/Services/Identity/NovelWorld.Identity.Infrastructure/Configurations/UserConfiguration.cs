using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovelWorld.Identity.Data.Entities;
using NovelWorld.Infrastructure.EntityFramework.Configurations;

namespace NovelWorld.Identity.Infrastructure.Configurations
{
    public class UserConfiguration: EntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);
            builder.ToTable("Users", schema: "identity");
        }
    }
}