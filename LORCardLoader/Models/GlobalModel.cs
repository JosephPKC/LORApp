namespace LORCardLoader.Models;

internal class GlobalModel : BaseModel
{
    public IEnumerable<KeywordModel> VocabTerms { get; set; } = [];
    public IEnumerable<KeywordModel> Keywords { get; set; } = [];
    public IEnumerable<RegionModel> Regions { get; set; } = [];
}