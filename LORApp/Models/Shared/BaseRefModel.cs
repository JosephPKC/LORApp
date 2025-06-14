namespace LORApp.Models.Shared;

internal abstract class BaseRefModel : BaseModel
{
  public string Name { get; set; } = string.Empty;
  /// <summary>
  /// Internal ref code
  /// Typically, lowercase, no whitespaces, dashes, etc.
  /// </summary>
  public string RefCode { get; set; } = string.Empty;
}