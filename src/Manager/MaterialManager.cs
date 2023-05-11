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
            material.price = (int)Svc.Data.GetExcelSheet<GilShopItem>().Where(x => x.Item.Row == material.id).Select(x => x.Item.Value.PriceMid).FirstOrDefault();
            if (material.price != 0)
            {
                material.worldName = "NPC商店";
            }
            
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