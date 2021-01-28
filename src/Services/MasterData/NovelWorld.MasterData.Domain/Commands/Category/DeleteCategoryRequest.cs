using System;
using FluentValidation;
using NovelWorld.Data.Requests;
using NovelWorld.Domain.Commands;

namespace NovelWorld.MasterData.Domain.Commands.Category
{
    public class DeleteCategoryRequest: ICommand, IRequest
    {
        public Guid Id { get; set; }
    }
    
    public class DeleteCategoryRequestValidator: AbstractValidator<DeleteCategoryRequest>
    {
        public DeleteCategoryRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}