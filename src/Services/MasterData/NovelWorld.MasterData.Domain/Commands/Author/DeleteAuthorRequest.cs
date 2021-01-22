using System;
using FluentValidation;
using NovelWorld.Data.Requests;
using NovelWorld.Domain.Commands;

namespace NovelWorld.MasterData.Domain.Commands.Author
{
    public class DeleteAuthorRequest: ICommand, IRequest
    {
        public Guid Id { get; set; }
    }
    
    public class DeleteAuthorRequestValidator: AbstractValidator<DeleteAuthorRequest>
    {
        public DeleteAuthorRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}