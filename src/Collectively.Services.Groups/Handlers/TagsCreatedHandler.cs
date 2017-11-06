using System;
using System.Linq;
using System.Threading.Tasks;
using Collectively.Common.Extensions;
using Collectively.Common.Services;
using Collectively.Messages.Events;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Groups.Dto;
using Collectively.Services.Groups.Services;

namespace Collectively.Services.Groups.Handlers
{
    public class TagsCreatedHandler : IEventHandler<TagsCreated>
    {
        private readonly IHandler _handler;
        private readonly ITagService _tagService;

        public TagsCreatedHandler(IHandler handler, ITagService tagService)
        {
            _handler = handler;
            _tagService = tagService;
        }

        public async Task HandleAsync(TagsCreated @event)
        {
            var tags = @event.Tags.Select(x => new TagDto
            {
                Id = x.Id,
                Name = x.Name,
                Translations = x.Translations.Select(t => new TranslatedTagDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Culture = t.Culture
                }).ToList()
            }).ToList();

            await _handler
                .Run(async () => await _tagService.AddOrUpdateAsync(tags))
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }
    }
}