﻿using PineapplePlanner.Domain.Enums;
using PineapplePlanner.Domain.JsonConverter;
using System.Text.Json.Serialization;

namespace PineapplePlanner.Domain.Dto.Gemini
{
    public class EntryDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }

        [JsonConverter(typeof(PriorityEnumConverter))]
        public Priority? Priority { get; set; }
        public DateTime? DateDue { get; set; }
        public List<string> Tags { get; set; } = [];
    }
}
