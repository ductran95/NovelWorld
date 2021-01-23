using System;
using FluentValidation;
using NovelWorld.Data.Requests;
using NovelWorld.Domain.Queries;
using NovelWorld.MasterData.Data.Responses;

namespace NovelWorld.MasterData.Domain.Queries.Category
{
    public class GetDetailCategoryRequest: IQuery<CategoryDetailResponse>, IRequest
    {
        public Guid Id { get; set; }
    }
    
    public class GetDetailCategoryRequestValidator: AbstractValidator<GetDetailCategoryRequest>
    {
        public GetDetailCategoryRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}