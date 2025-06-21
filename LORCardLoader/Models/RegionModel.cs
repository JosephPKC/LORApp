namespace LORCardLoader.Models;

internal class RegionModel : BaseModel
{
    public string Abbreviation { get; set; } = string.Empty;
    public string IconAbsolutePath { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string NameRef { get; set; } = string.Empty;
}
