using LORCardLoader.Models;

namespace LORCardLoader.Loader.Inserts;

internal class InsertBuilderFactory
{
    public static IInsertBuilder<TModel> CreateInsertBuilder<TModel>() where TModel : BaseModel
    {
        return typeof(TModel).Name switch
        {
            nameof(CardModel) => (IInsertBuilder<TModel>)new CardInsertBuilder(),
            nameof(KeywordModel) => (IInsertBuilder<TModel>)new KeywordInsertBuilder(),
            nameof(RegionModel) => (IInsertBuilder<TModel>)new RegionInsertBuilder(),
            _ => throw new NotSupportedException($"Type {typeof(TModel).Name} not supported.")
        };
    }

    public static IInsertBuilder<CardModel> CreateCardInsertBuilder(string pCardType)
    {
        return pCardType.ToUpper() switch
        {
            "CHAMPION" => new ChampionCardInsertBuilder(),
            "UNIT" => new UnitCardInsertBuilder(),
            _ => throw new NotSupportedException($"Card type {pCardType} not supported.")
        };
    }
}
