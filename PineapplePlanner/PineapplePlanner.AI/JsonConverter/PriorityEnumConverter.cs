using PineapplePlanner.Domain.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PineapplePlanner.AI.JsonConverter
{
    public class PriorityEnumConverter : JsonConverter<Priority?>
    {
        public override Priority? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? value = reader.GetString();
            if (!string.IsNullOrEmpty(value) && Enum.TryParse<Priority>(value, true, out Priority parsedPriority))
            {
                return parsedPriority;
            }
            return null;
        }

        public override void Write(Utf8JsonWriter writer, Priority? value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value?.ToString());
        }
    }
}
