using System.Text.Json;

namespace LORCardLoader.Loader.Parsing;

internal class JsonModelParser : IModelParser
{
    private JsonSerializerOptions Options { get; } = new()
    {
        PropertyNameCaseInsensitive = true
    };

    #region IModelParser
    public TModel? GetModel<TModel>(string pFile)
    {
        using StreamReader reader = new(pFile);
        string json = reader.ReadToEnd();
        return JsonSerializer.Deserialize<TModel>(json, Options);
    }

    public IEnumerable<TModel>? GetModels<TModel>(string pFile)
    {
        using StreamReader reader = new(pFile);
        string json = reader.ReadToEnd();
        Console.WriteLine("JSON: " + json);
        return JsonSerializer.Deserialize<IEnumerable<TModel>>(json, Options);
    }
    #endregion
}
