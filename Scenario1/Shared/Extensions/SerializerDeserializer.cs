using System.Text.Json;

namespace Shared.Extensions
{
    public static class SerializerDeserializer
    {
        public static string Serialize<T>(this T @object)
            => JsonSerializer.Serialize(@object);

        public static T? Deserialize<T>(this string @stringData)
            => JsonSerializer.Deserialize<T>(stringData);



    }
}
