using System.Text.Json;
using System.Text.Json.Serialization;

namespace Boxes.WebApi.Utils;

public class JsonStringConverter<TValue> : JsonConverter<TValue>
{
  public override TValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    if (reader.TokenType != JsonTokenType.String)
    {
      throw new JsonException($"Esperado un token de tipo String para deserializar {typeof(TValue).Name}.");
    }

    string jsonString = reader.GetString();

    if (string.IsNullOrEmpty(jsonString))
    {
      return default;
    }

    return JsonSerializer.Deserialize<TValue>(jsonString, options);
  }

  public override void Write(Utf8JsonWriter writer, TValue value, JsonSerializerOptions options)
  {
    string jsonString = JsonSerializer.Serialize(value, options);

    writer.WriteStringValue(jsonString);
  }
}
