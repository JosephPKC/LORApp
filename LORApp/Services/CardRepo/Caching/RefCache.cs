using LORApp.Entities.Refs;

namespace LORApp.Services.CardRepo.Caching;

/// <summary>
/// A cache for static data.
/// Not meant to be added to or changed. To change the cache, you need to create a new one.
/// </summary>
/// <param name="pKeywords"></param>
/// <param name="pVocab"></param>
/// <param name="pRegions"></param>
internal class RefCache(IEnumerable<KeywordModel> pKeywords, IEnumerable<KeywordModel> pVocab, IEnumerable<RegionModel> pRegions)
{
    public IReadOnlyDictionary<string, KeywordModel> Keywords { get; private set; } = pKeywords.ToDictionary(x => x.RefCode, x => x);
    public IReadOnlyDictionary<string, KeywordModel> Vocab { get; private set; } = pVocab.ToDictionary(x => x.RefCode, x => x);
    public IReadOnlyDictionary<string, RegionModel> Regions { get; private set; } = pRegions.ToDictionary(x => x.RefCode, x => x);
}