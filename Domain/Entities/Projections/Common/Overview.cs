
using System.Collections.Generic;

namespace Domain.Entities.Projections.Common;

public class Overview<T>
{
    public IList<T> Records { get; set; }

    public MetaData MetaData { get; set; }

    public Overview()
    {
        Records = new List<T>();
        MetaData = new MetaData();
    }

    public Overview(IList<T> records, MetaData metaData)
    {
        if (records != null)
            Records = records;
        else
            Records = new List<T>();

        MetaData = metaData;
    }
}
