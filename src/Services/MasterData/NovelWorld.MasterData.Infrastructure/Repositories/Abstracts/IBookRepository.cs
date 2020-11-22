using NovelWorld.Infrastructure.EntityFrameworkCore.Repositories.Abstractions;
using NovelWorld.MasterData.Data.Entities;

namespace NovelWorld.MasterData.Infrastructure.Repositories.Abstracts
{
    public interface IBookRepository: IEfCoreRepository<Book>
    {
        
    }
}