namespace Profiteering.DTO;
internal class TableRow(int id, string name, int unitPrice, int count, string worldName)
{
    internal int Id { get; set; } = id;
    internal string Name { get; set; } = name;
    internal int UnitPrice { get; set; } = unitPrice;
    internal int Count { get; set; } = count;
    internal int Total { get; set; } = unitPrice * count;
    internal string WorldName { get; set; } = worldName;
}