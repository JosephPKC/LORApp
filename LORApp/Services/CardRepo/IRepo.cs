using System.Data;

namespace LORApp.Services.CardRepo;

public interface IRepo
{
    /// <summary>
    /// A NULL fields parameter implies that it will retrieve all fields associated with the item.
    /// </summary>
    /// <param name="pTable"></param>
    /// <param name="pFields"></param>
    /// <param name="pFilters"></param>
    /// <returns></returns>
    DataTable Read(string pTable, IEnumerable<string>? pFields, IEnumerable<KeyValuePair<string, object>>? pFilters);
}
