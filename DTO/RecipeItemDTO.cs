using System.Collections.Generic;

namespace Profiteering.DTO;

internal class RecipeItemDTO
{

    public uint id { get; set; }
    public string name { get; set; }
    public int count { get; set; }
    public List<MaterialDTO> Materials { get; set; }


    public RecipeItemDTO(uint id, string name, int count, List<MaterialDTO> Materials)
    {
        this.id = id;
        this.name = name;
        this.count = count;
        this.Materials = Materials;
    }

}