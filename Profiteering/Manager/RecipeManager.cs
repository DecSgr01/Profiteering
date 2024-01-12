using System.Linq;
using Lumina.Excel.GeneratedSheets;

namespace Profiteering.Manager;
internal static class RecipeManager
{
    internal static Recipe? GetRecipebyItemId(uint itemId)
    {
        if (itemId != 0)
        {
            return null;
        }
        return Dalamud.DataManager.GetExcelSheet<Recipe>()!.Where(x => x.RowId != 0 && x.ItemResult.Row == itemId).FirstOrDefault();
    }
}