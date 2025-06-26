using System.Data;

namespace LORApp.Services.CardRepo.Mappers;

internal class MapperUtils
{
    public static IEnumerable<TModel> GetAllModels<TModel>(DataTable pDt, Func<DataRow, TModel> pMap)
    {
        List<TModel> models = [];
        foreach (DataRow dr in pDt.Rows)
        {
            models.Add(pMap(dr));
        }
        return models;
    }

    public static bool GetBool(DataRow pDr, string pField)
    {
        return pDr[pField].ToString() == "1";
    }

    public static TEnum? GetEnum<TEnum>(DataRow pDr, string pField) where TEnum : Enum
    {
        if (Enum.TryParse(typeof(TEnum), pDr[pField].ToString(), out object? result))
        {
            return (TEnum)result;
        }

        return default;
    }

    public static int GetInt(DataRow pDr, string pField)
    {
        return Convert.ToInt32(pDr[pField]);
    }

    public static string GetString(DataRow pDr, string pField)
    {
        return pDr[pField].ToString() ?? string.Empty;
    }
}
