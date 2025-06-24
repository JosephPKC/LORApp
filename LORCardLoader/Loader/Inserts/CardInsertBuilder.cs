using LORCardLoader.Models;

namespace LORCardLoader.Loader.Inserts;

internal class CardInsertBuilder : IInsertBuilder<CardModel>
{
    #region IInsertBuilder<CardModel>
    public IEnumerable<string> BuildInsertValues(CardModel pModel)
    {
        //  CardCode, Name, CardType, CardRarity, Cost, ArtistName, ArtImagePath, 
        //  Description, DescriptionFormatted, FlavorText, IsCollectible
        return [
            pModel.CardCode, pModel.Name, pModel.Type, pModel.RarityRef,
            pModel.Cost.ToString(), pModel.ArtistName, pModel.Assets.First().GameAbsolutePath,
            pModel.DescriptionRaw, pModel.Description, pModel.FlavorText,
            (pModel.Collectible ? 1 : 0).ToString()
        ];
    }
    #endregion
}
