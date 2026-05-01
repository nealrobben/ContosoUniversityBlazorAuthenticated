using System;

namespace WebUI.Client.Dtos.Common;

public record MetaDataDto(int PageNumber, int TotalPages, int PageSize, int TotalRecords, string CurrentSort, string SearchString)
{
    public MetaDataDto() : this(default, default, default, default, default, default)
    {
    }

    public MetaDataDto(int pageNumber, int totalRecords, int pageSize, string currentSort, string searchString) 
        : this(pageNumber, default, pageSize, totalRecords, currentSort, searchString)
    {
        var numberOfPages = (TotalRecords / (double)PageSize);
        TotalPages = (int)Math.Ceiling(numberOfPages);
    }
}