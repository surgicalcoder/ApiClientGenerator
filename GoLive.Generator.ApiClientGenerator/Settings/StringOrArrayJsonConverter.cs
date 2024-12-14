using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GoLive.Generator.ApiClientGenerator.Settings;

public class StringOrArrayJsonConverter : JsonConverter<List<string>>
{
    public override List<string> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            // Single string case
            return new List<string> { reader.GetString()! };
        }
        else if (reader.TokenType == JsonTokenType.StartArray)
        {
            // Array of strings case
            var strings = new List<string>();
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                if (reader.TokenType == JsonTokenType.String)
                {
                    strings.Add(reader.GetString()!);
                }
                else
                {
                    throw new JsonException("Expected a string in the array.");
                }
            }
            return strings;
        }
        throw new JsonException("Expected a string or an array of strings.");
    }

    public override void Write(Utf8JsonWriter writer, List<string> value, JsonSerializerOptions options)
    {
        // Always serialize as an array of strings
        writer.WriteStartArray();
        foreach (var str in value)
        {
            writer.WriteStringValue(str);
        }
        writer.WriteEndArray();
    }
}