using System.Collections.Generic;

namespace Profiteering.DTO;
internal class RecipeItem
{
    public nint ItemIcon { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public int AmountResult { get; set; }
    public int Count;
    public string WorldName { get; set; }
    public List<RecipeItem> Materials { get; set; }
    public RecipeItem(nint itemIcon, int id, string name, int amountResult, List<RecipeItem> materials)
    {
        ItemIcon = itemIcon;
        Id = id;
        Name = name;
        AmountResult = amountResult;
        Count = amountResult;
        Materials = materials;
    }

    public RecipeItem(int id, string name, int count)
    {
        Id = id;
        Name = name;
        Count = count;
    }

    public override string ToString()
    {
        string a = Materials == null ? "" : string.Join(",", Materials);
        return $"{{\"id\":\"{Id}\",\"name\":\"{Name}\",\"price\":\"{Price}\",\"amountResult\":\"{AmountResult}\",\"count\":\"{Count}\",\"materials\":[{a}]}}";
    }
}