namespace Profiteering.DTO;
internal class TableRow
{
    internal int Id { get; set; }
    internal string Name { get; set; }
    internal int UnitPrice { get; set; }
    internal int Count { get; set; }
    internal int Total { get; set; }
    internal string WorldName { get; set; }

    public TableRow(int id, string name, int unitPrice, int count, string worldName)
    {
        Id = id;
        Name = name;
        UnitPrice = unitPrice;
        Count = count;
        Total = unitPrice * count;
        WorldName = worldName;
    }
}