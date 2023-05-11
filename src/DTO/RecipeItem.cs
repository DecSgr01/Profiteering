using System.Collections.Generic;
using ImGuiScene;

namespace Profiteering.DTO;
internal class RecipeItem
{
    public TextureWrap ItemIcon { get; set; }
    public int id { get; set; }
    public string name { get; set; }
    public int price { get; set; }
    public int amountResult { get; set; }
    public int count;
    public string worldName { get; set; }
    public List<RecipeItem> materials { get; set; }
    public RecipeItem(TextureWrap itemIcon, int id, string name, int amountResult, List<RecipeItem> materials)
    {
        ItemIcon = itemIcon;
        this.id = id;
        this.name = name;
        this.amountResult = amountResult;
        this.count = amountResult;
        this.materials = materials;
    }

    public RecipeItem(int id, string name, int count)
    {
        this.id = id;
        this.name = name;
        this.count = count;
    }

    public override string ToString()
    {
        string a = materials == null ? "" : string.Join(",", materials);
        return $"{{\"id\":\"{id}\",\"name\":\"{name}\",\"price\":\"{price}\",\"amountResult\":\"{amountResult}\",\"count\":\"{count}\",\"materials\":[{a}]}}";
    }
}