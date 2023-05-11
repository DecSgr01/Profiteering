using System.Collections.Generic;
using Profiteering.DTO;

namespace Profiteering.Util;
internal class TableRowComparerUtil : IEqualityComparer<TableRow>
{
    public bool Equals(TableRow x, TableRow y)
    {
        if (x.id == y.id)
        {
            x.count += y.count;
        }
        return x.id == y.id;
    }

    public int GetHashCode(TableRow obj)
    {
        //返回字段的HashCode，只有HashCode相同才会去比较
        return obj.id.GetHashCode();
    }
}