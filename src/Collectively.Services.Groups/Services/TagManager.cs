using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Groups.Domain;
using Collectively.Services.Groups.Queries;
using Collectively.Services.Groups.Repositories;

namespace Collectively.Services.Groups.Services
{
    public class TagManager : ITagManager
    {
        private readonly ITagRepository _tagRepository;

        public TagManager(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<Maybe<IEnumerable<GroupTag>>> FindAsync(IEnumerable<Guid> tagsIds)
        {
            if (tagsIds == null || !tagsIds.Any())
            {
                return null;
            }
            var tags = await _tagRepository.BrowseAsync(new BrowseTags
            {
                Results = int.MaxValue
            });
            if (tags.HasNoValue)
            {
                return null;
            }
            var allTags = tags.Value.Items;
            var selectedTags = allTags.Where(x => tagsIds.Contains(x.Id) || x.Translations.Any(t => tagsIds.Contains(t.Id)));
            var availableTags = new HashSet<GroupTag>();
            foreach (var tagId in tagsIds)
            {
                var name = string.Empty;
                var @default = string.Empty;
                var defaultTag = allTags.SingleOrDefault(x => x.Id == tagId);
                if (defaultTag != null)
                {
                    name = defaultTag.Name;
                    @default = defaultTag.Name;
                    availableTags.Add(GroupTag.Create(tagId, name, defaultTag.Id, @default));
                    continue;
                }
                var translatedTag = allTags.SingleOrDefault(x => x.Translations.Any(t => t.Id == tagId));
                if (translatedTag != null)
                {
                    name = translatedTag.Translations.Single(x => x.Id == tagId).Name;
                    @default = translatedTag.Name;
                    availableTags.Add(GroupTag.Create(tagId, name, translatedTag.Id, @default));
                    continue;
                }
            }

            return availableTags;
        }
    }
}