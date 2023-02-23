using System.Collections.Generic;
using ImGuiScene;

namespace Profiteering.DTO;

internal class RecipeItemDTO
{
    public TextureWrap ItemIcon { get; set; }
    public uint id { get; set; }
    public string name { get; set; }
    public int count { get; set; }
    public int price { get; set; }
    public List<MaterialDTO> materials { get; set; }


    public RecipeItemDTO(uint id, string name, int count, TextureWrap ItemIcon, List<MaterialDTO> materials)
    {
        this.id = id;
        this.name = name;
        this.count = count;
        this.ItemIcon = ItemIcon;
        this.materials = materials;
    }

}