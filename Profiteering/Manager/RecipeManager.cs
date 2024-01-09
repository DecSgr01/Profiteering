using System.Linq;
using Lumina.Excel.GeneratedSheets;

namespace Profiteering.Manager;
internal static class RecipeManager
{
<<<<<<< HEAD
    internal static Recipe GetRecipebyItemId(int itemId)
    {
        return Dalamud.DataManager.GetExcelSheet<Recipe>().Where(x => x.RowId != 0 && itemId != 0 && x.ItemResult.Row == itemId).FirstOrDefault();
    }

=======
    internal static Recipe? GetRecipebyItemId(uint itemId)
    {
        if (itemId != 0)
        {
            return null;
        }
        return Dalamud.DataManager.GetExcelSheet<Recipe>()!.Where(x => x.RowId != 0 && x.ItemResult.Row == itemId).FirstOrDefault();
    }
>>>>>>> 17e02ca (api8)
}