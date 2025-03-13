using PineapplePlanner.Domain.Entities;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PineapplePlanner.Domain.JsonConverter
{
    public class TagListConverter : JsonConverter<List<Tag>?>
    {
        public override List<Tag>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException("Expected a JSON array.");
            }

            var tags = new List<Tag>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                    break;

                if (reader.TokenType == JsonTokenType.String)
                {
                    string? tagName = reader.GetString();
                    if (!string.IsNullOrEmpty(tagName))
                    {
                        tags.Add(new Tag
                        {
                            Id = string.Empty,
                            Name = tagName
                        });
                    }
                }
                else
                {
                    throw new JsonException("Expected a string inside the array.");
                }
            }

            return tags;
        }

        public override void Write(Utf8JsonWriter writer, List<Tag>? value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            if (value != null)
            {
                foreach (Tag tag in value)
                {
                    writer.WriteStringValue(tag.Name);
                }
            }

            writer.WriteEndArray();
        }
    }
}
