using System.Collections.Generic;
using ImGuiScene;

namespace Profiteering.DTO;
internal class TableRow
{
    public int id { get; set; }
    public string name { get; set; }
    public int unitPrice { get; set; }
    public int count { get; set; }
    public int total { get; set; }
    public string worldName { get; set; }

    public TableRow(int id, string name, int unitPrice, int count, string worldName)
    {
        this.id = id;
        this.name = name;
        this.unitPrice = unitPrice;
        this.count = count;
        this.total = unitPrice * count;
        this.worldName = worldName;
    }

    public override string ToString()
    {
        return $"{{\"id\": \"{id}\",\"name\":\" {name}\",\"unitPrice\":\" {unitPrice}\",\"count\":\" {count}\",\"total\":\" {total}\",\"worldName\":\" {worldName}\"}}";
    }
}