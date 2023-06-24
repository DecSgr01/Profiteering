using Dalamud.Interface;
using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using Dalamud.Utility;
using ECommons.DalamudServices;
using ImGuiNET;
using ImGuiScene;
using Lumina.Data.Files;
using Lumina.Excel.GeneratedSheets;
using Profiteering.Client;
using Profiteering.DTO;
using Profiteering.Manager;
using Profiteering.Response;
using Profiteering.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.Json;
using System.Threading.Tasks;

namespace Profiteering.View;

internal class ProfiteeringView : Window
{
    private ImGuiSortDirection SortDirectionr = 0;
    RecipeItem recipeItem;
    List<TableRow> tableRows;
    public ProfiteeringView() : base("ProfiteeringView", ImGuiWindowFlags.NoScrollbar)
    {
        Vector2 minSize = new Vector2(400, 220);
        this.Size = minSize;
        this.SizeCondition = ImGuiCond.FirstUseEver;
    }

    public override void Draw()
    {
        ImGui.BeginChild("", ImGui.GetContentRegionAvail() with { Y = ImGui.GetContentRegionAvail().Y - ImGuiHelpers.GetButtonSize("关闭").Y - 6 });

        ImGui.Image(recipeItem.ItemIcon.ImGuiHandle, new Vector2(40, 40));
        ImGui.SameLine();
        ImGui.Text($"{recipeItem.name}\n售价:{recipeItem.price}");
        ImGui.SameLine();
        double operatingRevenue = recipeItem.count * recipeItem.price;
        ImGui.Text($"\n    销售额:{operatingRevenue}");
        if (ImGui.Checkbox("基础素材", ref Profiteering.Instance.config.isBasicsMaterials))
        {
            Profiteering.Instance.saveConfig();
        }
        ImGui.SameLine();
        if (ImGui.Checkbox("HQ", ref Profiteering.Instance.config.isHq))
        {
            Profiteering.Instance.saveConfig();
            RefreshRecipePrice(Profiteering.Instance.config.isHq);
        }

        if (ImGui.InputInt(":个数", ref recipeItem.count, recipeItem.amountResult))
        {
            if (recipeItem.count <= 0) recipeItem.count = recipeItem.amountResult;
        }

        int num = recipeItem.count % recipeItem.amountResult > 0 ? (recipeItem.count / recipeItem.amountResult) + 1 : recipeItem.count / recipeItem.amountResult;

        tableRows = RefreshTableRow(recipeItem.materials, num);

        ImGuiTableFlags flags = ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.RowBg | ImGuiTableFlags.Borders | ImGuiTableFlags.Resizable | ImGuiTableFlags.Reorderable | ImGuiTableFlags.Hideable | ImGuiTableFlags.Sortable;
        if (ImGui.BeginTable("table", 6, flags))
        {
            ImGui.TableSetupColumn("id", ImGuiTableColumnFlags.DefaultHide | ImGuiTableColumnFlags.WidthFixed);
            ImGui.TableSetupColumn("名称", ImGuiTableColumnFlags.WidthFixed);
            ImGui.TableSetupColumn("数量", ImGuiTableColumnFlags.WidthFixed);
            ImGui.TableSetupColumn("平均单价", ImGuiTableColumnFlags.WidthFixed);
            ImGui.TableSetupColumn("总价", ImGuiTableColumnFlags.WidthFixed);
            ImGui.TableSetupColumn("区域", ImGuiTableColumnFlags.WidthStretch);
            ImGui.TableSetupScrollFreeze(0, 1);
            ImGui.TableHeadersRow();

            unsafe
            {
                var sorts_specs = ImGui.TableGetSortSpecs();
                if (sorts_specs.NativePtr != null && sorts_specs.SpecsDirty)
                {
                    this.SortDirectionr = sorts_specs.Specs.SortDirection;
                    sorts_specs.SpecsDirty = false;
                }
            }

            var items = this.SortDirectionr switch
            {
                ImGuiSortDirection.Ascending => tableRows.OrderBy(x => x.id),
                ImGuiSortDirection.Descending => tableRows.OrderByDescending(x => x.id),
                _ => tableRows.OrderByDescending(x => x.id)
            };

            foreach (var item in items)
            {
                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);
                ImGui.Text($"{item.id}");
                ImGui.TableSetColumnIndex(1);
                ImGui.PushID(item.id);
                bool selected = false;
                if (ImGui.Selectable($"{item.name}", ref selected))
                {
                    ImGui.SetClipboardText(item.name);
                }
                ImGui.PopID();
                ImGui.TableSetColumnIndex(2);
                ImGui.Text($"{item.count}");
                ImGui.TableSetColumnIndex(3);
                string v = item.unitPrice.ToString();
                ImGui.PushID(item.id);
                ImGui.SetNextItemWidth(ImGui.GetColumnWidth());
                if (ImGui.InputText("##value", ref v, 9))
                {
                    if (!int.TryParse(v, out int price)) price = 0;
                    setMaterialsPrice(recipeItem.materials, item.id, price);
                }
                ImGui.PopID();
                ImGui.TableSetColumnIndex(4);
                ImGui.Text($"{item.total}");
                ImGui.TableSetColumnIndex(5);
                ImGui.Text($"{item.worldName}");
            }
            ImGui.TableNextRow();
            ImGui.TableSetColumnIndex(1);
            ImGui.Text("总计");
            ImGui.TableSetColumnIndex(3);
            ImGui.Text($"{items.Sum(x => x.unitPrice)}");
            ImGui.TableSetColumnIndex(4);
            ImGui.Text($"{items.Sum(x => x.total)}");
            ImGui.EndTable();
        }
        ImGui.NewLine();
        double grossProfit = operatingRevenue - tableRows.Sum(x => x.total);
        double grossProfitMargin = grossProfit / operatingRevenue;
        double netProfit = operatingRevenue * 0.95 - tableRows.Sum(x => x.total);
        double netProfitMargin = netProfit / operatingRevenue;
        ImGui.Text($"毛利润:{grossProfit}");
        ImGui.SameLine();
        ImGui.Text($"毛利率:{grossProfitMargin.ToString("P")}%%");
        ImGui.Text($"净利润:{Math.Round(netProfit)}");
        ImGui.SameLine();
        ImGui.Text($"净利率:{netProfitMargin.ToString("P")}%%");
        ImGui.EndChild();
        ImGui.Separator();
        ImGui.NewLine();
        ImGui.SameLine(ImGui.GetContentRegionAvail().X - ImGuiHelpers.GetButtonSize("关闭").X - 10);

        if (ImGui.Button("关闭"))
        {
            IsOpen = false;
        }

        ImGui.End();
    }
    private List<TableRow> RefreshTableRow(List<RecipeItem> materials, int num)
    {
        List<TableRow> tableRow = new List<TableRow>();
        foreach (var material in materials)
        {
            if (Profiteering.Instance.config.isBasicsMaterials && material.amountResult != 0 && material.materials != null)
            {
                tableRow.AddRange(getBaseMaterial(material, num));
            }
            else
            {
                tableRow.Add(new TableRow(material.id, material.name, material.price, material.count * num, material.worldName));
            }
        }
        return tableRow.Distinct(new TableRowComparerUtil()).ToList();
    }
    private List<TableRow> getBaseMaterial(RecipeItem material, int num)
    {
        List<TableRow> tableRow = new List<TableRow>();
        int materialCount = (material.count * num) % material.amountResult > 0 ? ((material.count * num) / material.amountResult) + 1 : (material.count * num) / material.amountResult;

        if (material.amountResult != 0 && material.materials != null)
        {
            tableRow.AddRange(RefreshTableRow(material.materials, materialCount));
        }
        else
        {
            tableRow.Add(new TableRow(material.id, material.name, material.price, materialCount, material.worldName));
        }

        return tableRow.Distinct(new TableRowComparerUtil()).ToList();
    }
    public void profiteering(Recipe recipe)
    {

        List<RecipeItem> recipeItems = MaterialManager.getMaterials(recipe);

        TexFile texFile = Svc.Data.GetIcon(recipe.ItemResult.Value.Icon);
        TextureWrap textureWrap = Svc.PluginInterface.UiBuilder.LoadImageRaw(texFile.GetRgbaImageData(), texFile.Header.Width, texFile.Header.Height, 4);

        recipeItem = new RecipeItem(textureWrap, (int)recipe.ItemResult.Row, recipe.ItemResult.Value.Name.ToString(), recipe.AmountResult, recipeItems);

        tableRows = RefreshTableRow(recipeItem.materials, 1);

        PluginLog.Debug($"recipeItem:{recipeItem.ToString()}");
        PluginLog.Debug($"tableRows:{String.Join(",", tableRows)}");

        RefreshRecipePrice(Profiteering.Instance.config.isHq);
        RefreshMaterialsPrice();
        this.IsOpen = true;
    }
    private void RefreshRecipePrice(bool isHq)
    {
        Task.Run(async () =>
        {
            string word = Svc.ClientState.LocalPlayer.CurrentWorld.GameData.Name.ToString();
            Response.Item item = await UniversalisClient.GetRecipePriceAsync(recipeItem.id, word, isHq);
            PluginLog.Debug($"RecipeResponse:{JsonSerializer.Serialize(item)}");
            Listing listing = item.listings.FirstOrDefault();
            if (listing != null)
            {
                recipeItem.price = listing.pricePerUnit;
            }
        });
    }
    private void RefreshMaterialsPrice()
    {
        string word = Svc.ClientState.LocalPlayer?.CurrentWorld.GameData?.DataCenter.Value.Name.ToString();
        int[] ids = getMaterialsId(recipeItem.materials).ToArray();
        PluginLog.Debug($"ids:{String.Join(",", ids)}");
        Task.Run(async () =>
        {
            MarketDataResponse marketDataResponse = await UniversalisClient.GetMaterialsPriceAsync(ids, word);
            PluginLog.Debug($"MaterialsResponse:{JsonSerializer.Serialize(marketDataResponse)}");
            setMaterialsPrice(recipeItem.materials, marketDataResponse.items);
        });
    }
    private List<int> getMaterialsId(List<RecipeItem> materials)
    {
        List<int> ids = new List<int>();
        foreach (RecipeItem material in materials)
        {
            ids.Add(material.id);
            if (material.materials != null)
            {
                ids.AddRange(getMaterialsId(material.materials));
            }
        }
        return ids.Distinct().ToList();
    }
    private void setMaterialsPrice(List<RecipeItem> materials, Dictionary<int, Response.Item> items)
    {
        foreach (RecipeItem material in materials)
        {
            if (items.TryGetValue(material.id, out Response.Item item))
            {
                int price = 0;
                foreach (var Listing in item.listings)
                {
                    price += Listing.pricePerUnit;
                }
                price /= 10;
                if (material.price == 0 || material.price > price)
                {
                    material.price = price;
                    material.worldName = item.listings.FirstOrDefault().worldName;
                }
                if (material.materials != null)
                {
                    setMaterialsPrice(material.materials, items);
                }
            }
        }
    }
    private void setMaterialsPrice(List<RecipeItem> materials, int id, int price)
    {
        foreach (RecipeItem material in materials)
        {
            if (material.id == id)
            {
                material.price = price;
            }
            if (material.materials != null)
            {
                setMaterialsPrice(material.materials, id, price);
            }
        }
    }
}