using dotnetConf2023.Shared.Abstraction.Serialization;
using Jil;

namespace dotnetConf2023.Shared.Infrastructure.Serialization.Jil;

internal sealed class JilJsonSerializer : IJsonSerializer
{
    public string Serialize<T>(T value)
    {
        using var output = new StringWriter();
        JSON.Serialize(value, output);
        return output.ToString();
    }

    public T? Deserialize<T>(string value)
    {
        using var input = new StringReader(value);
        var result = JSON.Deserialize<T>(input);
        return result;
    }

    public object Deserialize(string value, Type type)
        => throw new NotImplementedException();
}