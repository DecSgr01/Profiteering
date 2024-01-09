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
        List<RecipeItem> materials = recipe.UnkData5.Where(x => x.AmountIngredient > 0).Select(x => new RecipeItem(x.ItemIngredient, Dalamud.DataManager.GetExcelSheet<Item>()?.GetRow((uint)x.ItemIngredient)?.Name.ToString(), x.AmountIngredient)).ToList<RecipeItem>();

        foreach (var material in materials)
        {
            material.Price = (int)Dalamud.DataManager.GetExcelSheet<GilShopItem>().Where(x => x.Item.Row == material.Id).Select(x => x.Item.Value.PriceMid).FirstOrDefault();
            if (material.Price != 0)
            {
                material.WorldName = "NPC商店";
            }

            Recipe materialRecipe = RecipeManager.GetRecipebyItemId(material.Id);
            if (materialRecipe != null)
            {
                material.AmountResult = materialRecipe.AmountResult;
                material.Materials = getMaterials(materialRecipe);
            }
        }
        return materials;
    }
}