using NovelWorld.Infrastructure.EntityFrameworkCore.Repositories.Abstractions;
using NovelWorld.MasterData.Data.DTO;

namespace NovelWorld.MasterData.Infrastructure.Repositories.Abstracts
{
    public interface ICategoryRepository: IEfCoreRepository<Category>
    {
        
    }
}