using System;
using FluentValidation;
using NovelWorld.Domain.Commands;

namespace NovelWorld.MasterData.Domain.Commands
{
    public class DeleteAuthorCommand: Command
    {
        public Guid Id { get; set; }
    }
    
    public class DeleteAuthorCommandValidator: AbstractValidator<DeleteAuthorCommand>
    {
        public DeleteAuthorCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}