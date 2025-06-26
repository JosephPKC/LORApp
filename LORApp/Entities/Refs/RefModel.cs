namespace LORApp.Entities.Refs;

public abstract class RefModel : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public string RefCode { get; set; } = string.Empty;
}
