using System.Collections.Generic;

namespace Profiteering.DTO;
internal class RecipeItem
{
    internal nint ItemIcon { get; set; } = 0;
    internal int Id { get; set; } = 0;
    internal string Name { get; set; } = null!;
    internal int Price { get; set; } = 0;
    internal int AmountResult { get; set; } = 0;
    internal int Count = 0;
    internal string WorldName { get; set; } = null!;
    internal List<RecipeItem> Materials { get; set; } = null!;
    internal RecipeItem(nint itemIcon, int id, string name, int amountResult, List<RecipeItem> materials)
    {
        ItemIcon = itemIcon;
        Id = id;
        Name = name;
        AmountResult = amountResult;
        Count = amountResult;
        Materials = materials;
    }

    internal RecipeItem(int id, string name, int count)
    {
        Id = id;
        Name = name;
        Count = count;
    }
}