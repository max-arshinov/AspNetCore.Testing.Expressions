namespace AspNetCore.Testing.Expressions;

public static class Extensions
{
    public static async Task<Dictionary<TKey, ListDetails<TList, TDetails>>> LoadMany<TKey, TList, TDetails>(
        this Task<IEnumerable<TList>?> listTask,
        Func<TList, Task<TDetails>> getDetails,
        Func<TList, TKey> listKey)
        where TKey : notnull
    {
        var list = await listTask;
        var res = list == null
            ? new Dictionary<TKey, ListDetails<TList, TDetails>>()
            // ReSharper disable once PossibleMultipleEnumeration
            : list.ToDictionary(listKey, x => new ListDetails<TList, TDetails>()
            {
                List = x
            });


        var detailsTasks = res
            .Select(async x =>
            {
                var value = await getDetails(x.Value.List);
                return new KeyValuePair<TKey, TDetails>(listKey(x.Value.List), value);
            })
            .ToArray();

        var details = await Task.WhenAll(detailsTasks);
        foreach (var detail in details)
        {
            if (res.ContainsKey(detail.Key))
            {
                res[detail.Key].Details = detail.Value;
            }
        }
        return res;
    }
}

public class ListDetails<TList, TDetails>
{
    public  TList List { get; internal set; }
    
    public TDetails Details { get; internal set; }
}