using MudBlazor;

namespace WebUINew.Client.Extensions;

public static class TableStateExtensions
{
    public static string GetSortString(this TableState value)
    {
        var sortField = value.SortLabel;
        var sortDirection = value.SortDirection == SortDirection.Ascending ? "asc" : "desc";
        return $"{sortField}_{sortDirection}";
    }
}