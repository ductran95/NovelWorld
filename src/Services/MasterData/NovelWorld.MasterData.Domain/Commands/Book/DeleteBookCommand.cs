using System;
using FluentValidation;
using NovelWorld.Domain.Commands;

namespace NovelWorld.MasterData.Domain.Commands
{
    public class DeleteBookCommand: Command
    {
        public Guid Id { get; set; }
    }
    
    public class DeleteBookCommandValidator: AbstractValidator<DeleteBookCommand>
    {
        public DeleteBookCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}