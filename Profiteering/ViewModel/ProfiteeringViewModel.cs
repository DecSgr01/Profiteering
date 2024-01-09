using Dalamud.Interface.Internal;
using Lumina.Excel.GeneratedSheets;
using Profiteering.Client;
using Profiteering.DTO;
using Profiteering.Manager;
using Profiteering.Response;
using Profiteering.Util;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Profiteering.ViewModel;

internal class ProfiteeringViewModel
{
    internal RecipeItem RecipeItem { get; set; } = null!;
    internal List<TableRow> TableRows { get; set; } = null!;
    internal List<TableRow> RefreshTableRow(List<RecipeItem> materials, int num)
    {
        List<TableRow> tableRow = new();
        foreach (var material in materials)
        {
            if (global::Profiteering.Profiteering.Config.isBasicsMaterials && material.AmountResult != 0 && material.Materials != null)
            {
                tableRow.AddRange(GetBaseMaterial(material, num));
            }
            else
            {
                tableRow.Add(new TableRow(material.Id, material.Name, material.Price, material.Count * num, material.WorldName));
            }
        }
        return tableRow.Distinct(new TableRowComparerUtil()).ToList();
    }
    private List<TableRow> GetBaseMaterial(RecipeItem material, int num)
    {
        List<TableRow> tableRow = new();
        int materialCount = material.Count * num % material.AmountResult > 0 ? (material.Count * num / material.AmountResult) + 1 : material.Count * num / material.AmountResult;

        if (material.AmountResult != 0 && material.Materials != null)
        {
            tableRow.AddRange(RefreshTableRow(material.Materials, materialCount));
        }
        else
        {
            tableRow.Add(new TableRow(material.Id, material.Name, material.Price, materialCount, material.WorldName));
        }

        return tableRow.Distinct(new TableRowComparerUtil()).ToList();
    }

    internal void Profiteering(Recipe recipe)
    {
        List<RecipeItem> recipeItems = MaterialManager.GetMaterials(recipe);
        IDalamudTextureWrap? dalamudTextureWrap = Dalamud.TextureProvider.GetIcon(recipe.ItemResult.Value!.Icon);
        RecipeItem = new RecipeItem(dalamudTextureWrap!.ImGuiHandle, (int)recipe.ItemResult.Row, recipe.ItemResult.Value.Name.ToString(), recipe.AmountResult, recipeItems);
        TableRows = RefreshTableRow(RecipeItem.Materials, 1);
        Dalamud.PluginLog.Debug($"recipeItem:{RecipeItem}");
        Dalamud.PluginLog.Debug($"tableRows:{string.Join(",", TableRows)}");
        RefreshRecipePrice(global::Profiteering.Profiteering.Config.isHq);
        RefreshMaterialsPrice();
    }
    internal void RefreshRecipePrice(bool isHq)
    {
        Task.Run(async () =>
        {
            string word = Dalamud.ClientState.LocalPlayer!.CurrentWorld.GameData!.Name.ToString();
            Response.Item item = await UniversalisClient.GetRecipePriceAsync(RecipeItem.Id, word, isHq);
            Dalamud.PluginLog.Debug($"RecipeResponse:{JsonSerializer.Serialize(item)}");
            Listing listing = item.Listings.FirstOrDefault()!;
            if (listing != null)
            {
                RecipeItem.Price = listing.PricePerUnit;
            }
        });
    }

    private void RefreshMaterialsPrice()
    {
        string word = Dalamud.ClientState.LocalPlayer!.CurrentWorld.GameData!.DataCenter.Value!.Name.ToString();
        int[] ids = GetMaterialsId(RecipeItem.Materials).ToArray();
        Dalamud.PluginLog.Debug($"ids:{string.Join(",", ids)}");
        Task.Run(async () =>
        {
            MarketDataResponse marketDataResponse = await UniversalisClient.GetMaterialsPriceAsync(ids, word);
            Dalamud.PluginLog.Debug($"MaterialsResponse:{JsonSerializer.Serialize(marketDataResponse)}");
            SetMaterialsPrice(RecipeItem.Materials, marketDataResponse.Items);
        });
    }
    private List<int> GetMaterialsId(List<RecipeItem> materials)
    {
        List<int> ids = GetMaterialsId(RecipeItem.Materials);
        foreach (RecipeItem material in materials)
        {
            ids.Add(material.Id);
            if (material.Materials != null)
            {
                ids.AddRange(GetMaterialsId(material.Materials));
            }
        }
        return ids.Distinct().ToList();
    }
    private void SetMaterialsPrice(List<RecipeItem> materials, Dictionary<int, Response.Item> items)
    {
        foreach (RecipeItem material in materials)
        {
            if (items.TryGetValue(material.Id, out Response.Item? item))
            {
                int price = 0;
                foreach (var Listing in item.Listings)
                {
                    price += Listing.PricePerUnit;
                }
                price /= 10;
                if (material.Price == 0 || material.Price > price)
                {
                    material.Price = price;
                    material.WorldName = item.Listings.FirstOrDefault()!.WorldName;
                }
                if (material.Materials != null)
                {
                    SetMaterialsPrice(material.Materials, items);
                }
            }
        }
    }
    public void SetMaterialsPrice(List<RecipeItem> materials, int id, int price)
    {
        foreach (RecipeItem material in materials)
        {
            if (material.Id == id)
            {
                material.Price = price;
            }
            if (material.Materials != null)
            {
                SetMaterialsPrice(material.Materials, id, price);
            }
        }
    }
}