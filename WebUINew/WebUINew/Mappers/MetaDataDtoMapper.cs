using Domain.Entities.Projections.Common;
using WebUINew.Client.Dtos.Common;

namespace WebUINew.Mappers;

public static class MetaDataDtoMapper
{
    public static MetaDataDto ToDto(MetaData model)
    {
        return new MetaDataDto
        {
            PageNumber = model.PageNumber,
            TotalPages = model.TotalPages,
            PageSize = model.PageSize,
            TotalRecords = model.TotalRecords,
            CurrentSort = model.CurrentSort,
            SearchString = model.SearchString
        };
    }
}