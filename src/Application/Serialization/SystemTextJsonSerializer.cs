using System.Text.Json;
using CleanBlazor.Application.Abstractions.Serialization;
using CleanBlazor.Application.Serialization.JsonConverters;

namespace CleanBlazor.Application.Serialization;

public class SystemTextJsonSerializer : IJsonSerializer
{
    private static readonly JsonSerializerOptions s_defaultOptions = new()
    {
        Converters = { new TimespanJsonConverter() }
    };

    public T Deserialize<T>(string text)
        => JsonSerializer.Deserialize<T>(text, s_defaultOptions);

    public string Serialize<T>(T obj)
        => JsonSerializer.Serialize(obj, s_defaultOptions);
}
