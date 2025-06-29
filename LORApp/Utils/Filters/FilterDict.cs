namespace LORApp.Utils.Filters;

internal class FilterDict<TKey> where TKey : notnull, Enum
{
    private Dictionary<TKey, bool> _filters = [];

    public void AddFilter(TKey pKey, bool pVal = true)
    {
        if (!_filters.ContainsKey(pKey))
        {
            _filters.Add(pKey, pVal);
        }
    }

    public void Set(TKey pKey, bool pVal)
    {
        if (!_filters.ContainsKey(pKey))
        {
            return;
        }

        _filters[pKey] = pVal;
    }

    public void SetAll(bool pVal)
    {
        foreach (TKey key in _filters.Keys)
        {
            _filters[key] = pVal;
        }
    }
}