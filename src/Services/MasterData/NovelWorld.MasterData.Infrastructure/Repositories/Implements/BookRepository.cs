using NovelWorld.Infrastructure.EntityFrameworkCore.Repositories.Implements;
using NovelWorld.MasterData.Data.Entities;
using NovelWorld.MasterData.Infrastructure.Contexts;
using NovelWorld.MasterData.Infrastructure.Repositories.Abstracts;

namespace NovelWorld.MasterData.Infrastructure.Repositories.Implements
{
    public class BookRepository: EfCoreRepository<MasterDataContext, Book>, IBookRepository
    {
        public BookRepository(MasterDataContext context) : base(context)
        {
        }
    }
}