using System;
using System.Collections.Generic;

namespace Collectively.Services.Groups.Dto
{
    public class TagDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<TranslatedTagDto> Translations { get; set; }        
    }
}