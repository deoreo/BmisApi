using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BmisApi.Services
{
    public class DateOnlyConverter : JsonConverter<DateOnly>
    {
        private string format = "MM/dd/yyyy";

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateOnly.ParseExact(reader.GetString(), format, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(format, CultureInfo.InvariantCulture));
        }
    }

    public class DateTimeConverter : JsonConverter<DateTime>
    {
        private const string Format = "MM/dd/yyyy HH:mm:ss";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.ParseExact(reader.GetString(), Format, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format, CultureInfo.InvariantCulture));
        }
    }
}
