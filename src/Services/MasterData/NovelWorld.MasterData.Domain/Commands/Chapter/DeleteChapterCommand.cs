using System;
using FluentValidation;
using NovelWorld.Domain.Commands;

namespace NovelWorld.MasterData.Domain.Commands
{
    public class DeleteChapterCommand: Command
    {
        public Guid Id { get; set; }
    }
    
    public class DeleteChapterCommandValidator: AbstractValidator<DeleteChapterCommand>
    {
        public DeleteChapterCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}