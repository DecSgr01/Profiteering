using System.Collections.Generic;
using System.Linq;
using ECommons.DalamudServices;
using Lumina.Excel.GeneratedSheets;
using Profiteering.DTO;

namespace Profiteering.Manager;
internal static class MaterialManager
{
    internal static List<MaterialDTO> getDirectMaterials(Recipe recipe)
    {
        List<MaterialDTO> materialDTOs = recipe.UnkData5.Where(x => x.AmountIngredient > 0).Select(x => new MaterialDTO(x.ItemIngredient, Svc.Data.GetExcelSheet<Item>()?.GetRow((uint)x.ItemIngredient)?.Name.ToString(), x.AmountIngredient)).ToList<MaterialDTO>();
        return materialDTOs;
    }
    internal static List<MaterialDTO> getBasicsMaterials(Recipe recipe)
    {
        List<MaterialDTO> DirectMaterials = getDirectMaterials(recipe);
        foreach (var DirectMaterial in DirectMaterials)
        {
            Recipe recipe1 = RecipeManager.getRecipebyItemId(DirectMaterial.id);
            if (recipe1 != null)
            {
                DirectMaterials = DirectMaterials.Where(x => x.id != DirectMaterial.id).ToList();
                DirectMaterials.AddRange(getBasicsMaterials(recipe1));
                DirectMaterials = DirectMaterials.GroupBy(x => new { x.id, x.name }).Select(i => new MaterialDTO(i.Key.id, i.Key.name, (byte)i.Sum(o => o.count))).ToList();
            }
        }
        return DirectMaterials;
    }
}