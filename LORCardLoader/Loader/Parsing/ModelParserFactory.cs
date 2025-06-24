namespace LORCardLoader.Loader.Parsing;

internal static class ModelParserFactory
{
    public static IModelParser CreateModelParser()
    {
        return new JsonModelParser();
    }
}
