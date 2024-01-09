namespace Profiteering.DTO;
internal class TableRow
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int UnitPrice { get; set; }
    public int Count { get; set; }
    public int Total { get; set; }
    public string WorldName { get; set; }

    public TableRow(int id, string name, int unitPrice, int count, string worldName)
    {
        Id = id;
        Name = name;
        UnitPrice = unitPrice;
        Count = count;
        Total = unitPrice * count;
        WorldName = worldName;
    }

    public override string ToString()
    {
        return $"{{\"id\": \"{Id}\",\"name\":\" {Name}\",\"unitPrice\":\" {UnitPrice}\",\"count\":\" {Count}\",\"total\":\" {Total}\",\"worldName\":\" {WorldName}\"}}";
    }
}