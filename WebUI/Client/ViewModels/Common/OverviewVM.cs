using System.Collections.Generic;

namespace WebUI.Client.ViewModels.Common;

public class OverviewVM<T>
{
    public IList<T> Records { get; set; }

    public MetaDataVM MetaData { get; set; }

    public OverviewVM()
    {
        Records = new List<T>();
        MetaData = new MetaDataVM();
    }

    public OverviewVM(IList<T> records, MetaDataVM metaData)
    {
        if (records != null)
            Records = records;
        else
            Records = new List<T>();

        MetaData = metaData;
    }
}
