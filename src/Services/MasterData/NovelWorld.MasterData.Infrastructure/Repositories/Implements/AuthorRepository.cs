using NovelWorld.Infrastructure.EntityFrameworkCore.Contexts;
using NovelWorld.Infrastructure.EntityFrameworkCore.Repositories.Implements;
using NovelWorld.MasterData.Data.DTO;
using NovelWorld.MasterData.Infrastructure.Contexts;
using NovelWorld.MasterData.Infrastructure.Repositories.Abstracts;

namespace NovelWorld.MasterData.Infrastructure.Repositories.Implements
{
    public class AuthorRepository: EfCoreRepository<MasterDataContext, Author>, IAuthorRepository
    {
        public AuthorRepository(EfCoreEntityContext context) : base(context)
        {
        }
    }
}