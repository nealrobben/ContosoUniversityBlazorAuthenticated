using System.Collections.Generic;

namespace WebUI.Client.Dtos.Common;

public record OverviewDto<T>(IList<T> Records, MetaDataDto MetaData)
{
    public OverviewDto() : this([], new MetaDataDto())
    {
    }
}