using LORCardLoader.Models;

namespace LORCardLoader.Loader.Inserts;

internal class CardInsertBuilder : IInsertBuilder<CardModel>
{
    #region IInsertBuilder<CardModel>
    public IEnumerable<string> BuildInsertValues(CardModel pModel)
    {
        //  CardCode, Name, CardType, CardRarity, Cost, ArtistName, ArtImagePath, Description, FlavorText
        return [
            pModel.CardCode, pModel.Name, pModel.Type, pModel.RarityRef,
            pModel.Cost.ToString(), pModel.ArtistName, pModel.Assets.First().GameAbsolutePath,
            pModel.Description, pModel.FlavorText
        ];
    }
    #endregion
}
