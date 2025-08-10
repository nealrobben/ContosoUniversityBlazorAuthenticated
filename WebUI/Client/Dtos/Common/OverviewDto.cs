using System.Collections.Generic;

namespace WebUI.Client.Dtos.Common;

public class OverviewDto<T>
{
    public IList<T> Records { get; set; }

    public MetaDataDto MetaData { get; set; }

    public OverviewDto()
    {
        Records = new List<T>();
        MetaData = new MetaDataDto();
    }

    public OverviewDto(IList<T> records, MetaDataDto metaData)
    {
        Records = records ?? new List<T>();

        MetaData = metaData;
    }
}
