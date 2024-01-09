using System.Collections.Generic;
using Profiteering.DTO;

namespace Profiteering.Util;
internal class TableRowComparerUtil : IEqualityComparer<TableRow>
{
    public bool Equals(TableRow? x, TableRow? y)
    {
<<<<<<< HEAD
        if (x.Id == y.Id)
=======
        if (x!.Id == y!.Id)
>>>>>>> 17e02ca (api8)
        {
            x.Count += y.Count;
        }
        return x.Id == y.Id;
    }

    public int GetHashCode(TableRow obj)
    {
        //返回字段的HashCode，只有HashCode相同才会去比较
        return obj.Id.GetHashCode();
    }
}