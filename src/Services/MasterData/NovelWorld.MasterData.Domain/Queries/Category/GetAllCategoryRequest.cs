using System.Collections.Generic;
using FluentValidation;
using NovelWorld.Data.Requests;
using NovelWorld.Domain.Queries;
using NovelWorld.MasterData.Data.Responses;

namespace NovelWorld.MasterData.Domain.Queries.Category
{
    public class GetAllCategoryRequest: IQuery<List<CategoryGeneralResponse>>, IRequest
    {
    }
    
    public class GetAllCategoryRequestValidator: AbstractValidator<GetAllCategoryRequest>
    {
        public GetAllCategoryRequestValidator()
        {
        }
    }
}