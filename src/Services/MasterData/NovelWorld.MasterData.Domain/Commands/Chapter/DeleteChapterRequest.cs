using System;
using FluentValidation;
using NovelWorld.Data.Requests;
using NovelWorld.Domain.Commands;

namespace NovelWorld.MasterData.Domain.Commands.Chapter
{
    public class DeleteChapterRequest: ICommand, IRequest
    {
        public Guid Id { get; set; }
    }
    
    public class DeleteChapterRequestValidator: AbstractValidator<DeleteChapterRequest>
    {
        public DeleteChapterRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}