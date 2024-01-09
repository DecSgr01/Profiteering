namespace Profiteering.DTO;
internal class TableRow
{
<<<<<<< HEAD
    public int Id { get; set; }
    public string Name { get; set; }
    public int UnitPrice { get; set; }
    public int Count { get; set; }
    public int Total { get; set; }
    public string WorldName { get; set; }
=======
    internal int Id { get; set; }
    internal string Name { get; set; }
    internal int UnitPrice { get; set; }
    internal int Count { get; set; }
    internal int Total { get; set; }
    internal string WorldName { get; set; }
>>>>>>> 17e02ca (api8)

    public TableRow(int id, string name, int unitPrice, int count, string worldName)
    {
        Id = id;
        Name = name;
        UnitPrice = unitPrice;
        Count = count;
        Total = unitPrice * count;
        WorldName = worldName;
    }
<<<<<<< HEAD

    public override string ToString()
    {
        return $"{{\"id\": \"{Id}\",\"name\":\" {Name}\",\"unitPrice\":\" {UnitPrice}\",\"count\":\" {Count}\",\"total\":\" {Total}\",\"worldName\":\" {WorldName}\"}}";
    }
=======
>>>>>>> 17e02ca (api8)
}