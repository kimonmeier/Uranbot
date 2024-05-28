using Newtonsoft.Json.Linq;
using JsonException = Newtonsoft.Json.JsonException;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace UranBot.Public.Database.Entities.Base;

public class InterfaceConverter<T> : Newtonsoft.Json.JsonConverter<T>
    where T : class
{
    private const string TypeName = "$type";
    private const string ValueName = "$value";
    
    public override void WriteJson(JsonWriter writer, T? value, JsonSerializer serializer)
    {
        switch (value)
        {
            case null:
                serializer.Serialize(writer, null);

                break;
            default:
                var type = value.GetType();
                
                writer.WriteStartObject();
                writer.WritePropertyName(TypeName);
                writer.WriteValue($"{type.Assembly.FullName}::{type.FullName}");
                writer.WritePropertyName(ValueName);
                
                serializer.Serialize(writer, value);

                writer.WriteEndObject();

                break;
        }
    }

    public override T? ReadJson(JsonReader reader, Type objectType, T? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.StartObject)
        {
            throw new JsonException();
        }

        string typeValue = GetJsonValue(reader, TypeName);

        var typeInfo = new
        {
            AssemblyName = typeValue.Split("::").First(), TypeName = typeValue.Split("::").Last()
        };
        
        var instance = Activator.CreateInstance(typeInfo.AssemblyName, typeInfo.TypeName)!.Unwrap();
        var entityType = instance!.GetType();

        reader.Read();
        reader.Read();
        JObject jObject = JObject.Load(reader, new JsonLoadSettings());
        
        var deserialized = jObject.ToObject(entityType);

        reader.Read();

        return (T)deserialized;
    }

    private static string GetJsonValue(JsonReader reader, string propertyName)
    {
        reader.Read();

        string propertyValue = reader.Path.Split('.').Last();
        if (propertyValue != propertyName)
        {
            throw new JsonException();
        }

        if (reader.TokenType != JsonToken.PropertyName)
        {
            throw new JsonException();
        }

        string typeValue = reader.ReadAsString() ?? string.Empty;
        if (reader.TokenType != JsonToken.String)
        {
            throw new JsonException();
        }

        return typeValue;
    }
    
    /*
     *    
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Utf8JsonReader readerClone = reader;
        if (readerClone.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        readerClone.Read();
        if (readerClone.TokenType != JsonTokenType.PropertyName)
        {
            throw new JsonException();
        }

        string propertyName = readerClone.GetString();
        if (propertyName != "$type")
        {
            throw new JsonException();
        }

        readerClone.Read();
        if (readerClone.TokenType != JsonTokenType.String)
        {
            throw new JsonException();
        }

        string typeValue = readerClone.GetString();
        var typeInfo = new
        {
            AssemblyName = typeValue.Split("::").First(), TypeName = typeValue.Split("::").Last()
        };
        var instance = Activator.CreateInstance(typeInfo.AssemblyName, typeInfo.TypeName).Unwrap();
        var entityType = instance.GetType();

        var deserialized = JsonSerializer.Deserialize(ref reader, entityType, options);

        return (T)deserialized;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case null:
                JsonSerializer.Serialize(writer, (T)null, options);

                break;
            default:
                var type = value.GetType();
                using (var jsonDocument = JsonDocument.Parse(JsonSerializer.Serialize(value, type, options)))
                {
                    writer.WriteStartObject();
                    writer.WriteString("$type", $"{type.Assembly.FullName}::{type.FullName}");

                    foreach (var element in jsonDocument.RootElement.EnumerateObject())
                    {
                        element.WriteTo(writer);
                    }

                    writer.WriteEndObject();
                }

                break;
        }
    }
     * *
     */
}