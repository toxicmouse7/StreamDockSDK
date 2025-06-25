using System.Text.Json;
using System.Text.Json.Serialization;

namespace StreamDockSDK.Extensions;

public static class JsonExtensions
{
    private static JsonSerializerOptions Options { get; } = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
    };
    
    public static string ToStreamDockJson(this object obj)
    {
        return JsonSerializer.Serialize(obj, Options);
    }

    public static TType FromStreamDockJson<TType>(this string obj)
    {
        return JsonSerializer.Deserialize<TType>(obj, Options)!;
    }
}