namespace LORCardLoader.Loader.Parsing;

internal interface IModelParser
{
    TModel? GetModel<TModel>(string pFile);
    IEnumerable<TModel>? GetModels<TModel>(string pFile);
}
