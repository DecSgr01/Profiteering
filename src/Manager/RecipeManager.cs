using System.Linq;
using ECommons.DalamudServices;
using Lumina.Excel.GeneratedSheets;

namespace Profiteering.Manager;
internal static class RecipeManager
{
    internal static Recipe getRecipebyItemId(int itemId)
    {
        return (Recipe)(Svc.Data.GetExcelSheet<Recipe>().Where(x => x.RowId != 0 && itemId != 0 && x.ItemResult.Row == itemId)).FirstOrDefault();
    }

}