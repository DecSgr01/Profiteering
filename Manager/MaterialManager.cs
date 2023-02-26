using System.Collections.Generic;
using System.Linq;
using ECommons.DalamudServices;
using Lumina.Excel.GeneratedSheets;
using Profiteering.DTO;

namespace Profiteering.Manager;
internal static class MaterialManager
{
    internal static List<RecipeItem> getMaterials(Recipe recipe)
    {
        List<RecipeItem> materials = recipe.UnkData5.Where(x => x.AmountIngredient > 0).Select(x => new RecipeItem(x.ItemIngredient, Svc.Data.GetExcelSheet<Item>()?.GetRow((uint)x.ItemIngredient)?.Name.ToString(), x.AmountIngredient)).ToList<RecipeItem>();

        foreach (var material in materials)
        {
            Recipe materialRecipe = RecipeManager.getRecipebyItemId(material.id);
            if (materialRecipe != null)
            {
                material.amountResult = materialRecipe.AmountResult;
                material.materials = getMaterials(materialRecipe);
            }
        }
        return materials;
    }
    
}