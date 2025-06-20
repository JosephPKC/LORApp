namespace LORCardLoader.Models;

internal class AssetModel : BaseModel
{
    public string GameAbsolutePath { get; set; } = string.Empty;
    public string FullAbsolutePath { get; set; } = string.Empty;
}
