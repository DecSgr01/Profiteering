using System.Collections.Generic;

namespace Profiteering.DTO;
internal class RecipeItem
{
<<<<<<< HEAD
    public nint ItemIcon { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public int AmountResult { get; set; }
    public int Count;
    public string WorldName { get; set; }
    public List<RecipeItem> Materials { get; set; }
    public RecipeItem(nint itemIcon, int id, string name, int amountResult, List<RecipeItem> materials)
=======
    internal nint ItemIcon { get; set; } = 0;
    internal int Id { get; set; } = 0;
    internal string Name { get; set; } = null!;
    internal int Price { get; set; } = 0;
    internal int AmountResult { get; set; } = 0;
    internal int Count = 0;
    internal string WorldName { get; set; } = null!;
    internal List<RecipeItem> Materials { get; set; } = null!;
    internal RecipeItem(nint itemIcon, int id, string name, int amountResult, List<RecipeItem> materials)
>>>>>>> 17e02ca (api8)
    {
        ItemIcon = itemIcon;
        Id = id;
        Name = name;
        AmountResult = amountResult;
        Count = amountResult;
        Materials = materials;
    }

<<<<<<< HEAD
    public RecipeItem(int id, string name, int count)
=======
    internal RecipeItem(int id, string name, int count)
>>>>>>> 17e02ca (api8)
    {
        Id = id;
        Name = name;
        Count = count;
    }
<<<<<<< HEAD

    public override string ToString()
    {
        string a = Materials == null ? "" : string.Join(",", Materials);
        return $"{{\"id\":\"{Id}\",\"name\":\"{Name}\",\"price\":\"{Price}\",\"amountResult\":\"{AmountResult}\",\"count\":\"{Count}\",\"materials\":[{a}]}}";
    }
=======
>>>>>>> 17e02ca (api8)
}