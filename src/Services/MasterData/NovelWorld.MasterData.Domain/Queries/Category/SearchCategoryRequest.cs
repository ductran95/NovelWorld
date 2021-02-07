using FluentValidation;
using NovelWorld.Domain.Queries;
using NovelWorld.MasterData.Data.Responses;
using NovelWorld.MasterData.Data.Responses.Category;

namespace NovelWorld.MasterData.Domain.Queries.Category
{
    public class SearchCategoryRequest: SearchRequest<CategoryGeneralResponse>
    {
    }
    
    public class SearchCategoryRequestValidator: AbstractValidator<SearchCategoryRequest>
    {
        public SearchCategoryRequestValidator()
        {
            
        }
    }
}