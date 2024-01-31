using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WFEngine.Domain.Common.JsonSerializer
{
    public static class JsonSerializerExtensions
    {
        private static JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = true
        };

        public static async Task<string> ToJson<T>(this T obj)
        {
            string json = string.Empty;
            using var stream = new MemoryStream();
            await System.Text.Json.JsonSerializer.SerializeAsync(stream, obj, obj.GetType(), _jsonSerializerOptions);
            stream.Position = 0;
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        public static async Task<T> ToObject<T>(this string json)
        {
            using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            using (StreamReader reader = new StreamReader(stream))
            using (JsonDocument document = await JsonDocument.ParseAsync(reader.BaseStream))
                return System.Text.Json.JsonSerializer.Deserialize<T>(document.RootElement.GetRawText(),_jsonSerializerOptions);
        }
    }
}
