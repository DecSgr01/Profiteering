<<<<<<< HEAD
using Dalamud.Interface.Internal;
=======
>>>>>>> 17e02ca (api8)
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;
<<<<<<< HEAD
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
=======
using Profiteering.ViewModel;
using System;
using System.Linq;
using System.Numerics;
>>>>>>> 17e02ca (api8)

namespace Profiteering.View;

internal class ProfiteeringView : Window
{
    private ImGuiSortDirection SortDirectionr = 0;
<<<<<<< HEAD
    private RecipeItem recipeItem;
    private List<TableRow> tableRows;
    public ProfiteeringView() : base("ProfiteeringView", ImGuiWindowFlags.NoScrollbar)
    {
        Vector2 minSize = new Vector2(400, 220);
        this.Size = minSize;
        this.SizeCondition = ImGuiCond.FirstUseEver;
=======
    private readonly ProfiteeringViewModel PVM;
    internal ProfiteeringView(string name, ProfiteeringViewModel profiteeringViewModel) : base(name, ImGuiWindowFlags.NoScrollbar)
    {
        PVM = profiteeringViewModel;
        Size = new(400, 220);
        SizeCondition = ImGuiCond.FirstUseEver;
>>>>>>> 17e02ca (api8)
    }

    public override void Draw()
    {
        ImGui.BeginChild("", ImGui.GetContentRegionAvail() with { Y = ImGui.GetContentRegionAvail().Y - ImGuiHelpers.GetButtonSize("关闭").Y - 6 });

<<<<<<< HEAD
        ImGui.Image(recipeItem.ItemIcon, new Vector2(40, 40));
        ImGui.SameLine();
        ImGui.Text($"{recipeItem.Name}\n售价:{recipeItem.Price}");
        ImGui.SameLine();
        double operatingRevenue = recipeItem.Count * recipeItem.Price;
        ImGui.Text($"\n    销售额:{operatingRevenue}");
        if (ImGui.Checkbox("基础素材", ref Profiteering.Config.isBasicsMaterials))
        {
            Profiteering.Config.Save();
        }
        ImGui.SameLine();
        if (ImGui.Checkbox("HQ", ref Profiteering.Config.isHq))
        {
            Profiteering.Config.Save();
            RefreshRecipePrice(Profiteering.Config.isHq);
        }

        if (ImGui.InputInt(":个数", ref recipeItem.Count, recipeItem.AmountResult))
        {
            if (recipeItem.Count <= 0) recipeItem.Count = recipeItem.AmountResult;
        }

        int num = recipeItem.Count % recipeItem.AmountResult > 0 ? (recipeItem.Count / recipeItem.AmountResult) + 1 : recipeItem.Count / recipeItem.AmountResult;

        tableRows = RefreshTableRow(recipeItem.Materials, num);
=======
        ImGui.Image(PVM.RecipeItem.ItemIcon, new Vector2(40, 40));
        ImGui.SameLine();
        ImGui.Text($"{PVM.RecipeItem.Name}\n售价:{PVM.RecipeItem.Price}");
        ImGui.SameLine();
        double operatingRevenue = PVM.RecipeItem.Count * PVM.RecipeItem.Price;
        ImGui.Text($"\n    销售额:{operatingRevenue}");
        if (ImGui.Checkbox("基础素材", ref global::Profiteering.Profiteering.Config.isBasicsMaterials))
        {
            global::Profiteering.Profiteering.Config.Save();
        }
        ImGui.SameLine();
        if (ImGui.Checkbox("HQ", ref global::Profiteering.Profiteering.Config.isHq))
        {
            global::Profiteering.Profiteering.Config.Save();
            PVM.RefreshRecipePrice(global::Profiteering.Profiteering.Config.isHq);
        }

        if (ImGui.InputInt(":个数", ref PVM.RecipeItem.Count, PVM.RecipeItem.AmountResult))
        {
            if (PVM.RecipeItem.Count <= 0) PVM.RecipeItem.Count = PVM.RecipeItem.AmountResult;
        }

        int num = PVM.RecipeItem.Count % PVM.RecipeItem.AmountResult > 0 ? (PVM.RecipeItem.Count / PVM.RecipeItem.AmountResult) + 1 : PVM.RecipeItem.Count / PVM.RecipeItem.AmountResult;

        PVM.TableRows = PVM.RefreshTableRow(PVM.RecipeItem.Materials, num);
>>>>>>> 17e02ca (api8)

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
                    SortDirectionr = sorts_specs.Specs.SortDirection;
                    sorts_specs.SpecsDirty = false;
                }
            }

            var items = SortDirectionr switch
            {
<<<<<<< HEAD
                ImGuiSortDirection.Ascending => tableRows.OrderBy(x => x.Id),
                ImGuiSortDirection.Descending => tableRows.OrderByDescending(x => x.Id),
                _ => tableRows.OrderByDescending(x => x.Id)
=======
                ImGuiSortDirection.Ascending => PVM.TableRows.OrderBy(x => x.Id),
                ImGuiSortDirection.Descending => PVM.TableRows.OrderByDescending(x => x.Id),
                _ => PVM.TableRows.OrderByDescending(x => x.Id)
>>>>>>> 17e02ca (api8)
            };

            foreach (var item in items)
            {
                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);
                ImGui.Text($"{item.Id}");
                ImGui.TableSetColumnIndex(1);
                ImGui.PushID(item.Id);
                bool selected = false;
                if (ImGui.Selectable($"{item.Name}", ref selected))
                {
                    ImGui.SetClipboardText(item.Name);
                }
                ImGui.PopID();
                ImGui.TableSetColumnIndex(2);
                ImGui.Text($"{item.Count}");
                ImGui.TableSetColumnIndex(3);
                string v = item.UnitPrice.ToString();
                ImGui.PushID(item.Id);
                ImGui.SetNextItemWidth(ImGui.GetColumnWidth());
                if (ImGui.InputText("##value", ref v, 9))
                {
                    if (!int.TryParse(v, out int price)) price = 0;
<<<<<<< HEAD
                    setMaterialsPrice(recipeItem.Materials, item.Id, price);
=======
                    PVM.SetMaterialsPrice(PVM.RecipeItem.Materials, item.Id, price);
>>>>>>> 17e02ca (api8)
                }
                ImGui.PopID();
                ImGui.TableSetColumnIndex(4);
                ImGui.Text($"{item.Total}");
                ImGui.TableSetColumnIndex(5);
                ImGui.Text($"{item.WorldName}");
            }
            ImGui.TableNextRow();
            ImGui.TableSetColumnIndex(1);
            ImGui.Text("总计");
            ImGui.TableSetColumnIndex(3);
            ImGui.Text($"{items.Sum(x => x.UnitPrice)}");
            ImGui.TableSetColumnIndex(4);
            ImGui.Text($"{items.Sum(x => x.Total)}");
            ImGui.EndTable();
        }
        ImGui.NewLine();
<<<<<<< HEAD
        double grossProfit = operatingRevenue - tableRows.Sum(x => x.Total);
        double grossProfitMargin = grossProfit / operatingRevenue;
        double netProfit = operatingRevenue * 0.95 - tableRows.Sum(x => x.Total);
=======
        double grossProfit = operatingRevenue - PVM.TableRows.Sum(x => x.Total);
        double grossProfitMargin = grossProfit / operatingRevenue;
        double netProfit = operatingRevenue * 0.95 - PVM.TableRows.Sum(x => x.Total);
>>>>>>> 17e02ca (api8)
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
<<<<<<< HEAD
    private List<TableRow> RefreshTableRow(List<RecipeItem> materials, int num)
    {
        List<TableRow> tableRow = new List<TableRow>();
        foreach (var material in materials)
        {
            if (Profiteering.Config.isBasicsMaterials && material.AmountResult != 0 && material.Materials != null)
            {
                tableRow.AddRange(getBaseMaterial(material, num));
            }
            else
            {
                tableRow.Add(new TableRow(material.Id, material.Name, material.Price, material.Count * num, material.WorldName));
            }
        }
        return tableRow.Distinct(new TableRowComparerUtil()).ToList();
    }
    private List<TableRow> getBaseMaterial(RecipeItem material, int num)
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

    public void profiteering(Recipe recipe)
    {

        List<RecipeItem> recipeItems = MaterialManager.getMaterials(recipe);

        IDalamudTextureWrap dalamudTextureWrap = Dalamud.TextureProvider.GetIcon(recipe.ItemResult.Value.Icon);

        recipeItem = new RecipeItem(dalamudTextureWrap.ImGuiHandle, (int)recipe.ItemResult.Row, recipe.ItemResult.Value.Name.ToString(), recipe.AmountResult, recipeItems);

        tableRows = RefreshTableRow(recipeItem.Materials, 1);

        Dalamud.PluginLog.Debug($"recipeItem:{recipeItem}");
        Dalamud.PluginLog.Debug($"tableRows:{string.Join(",", tableRows)}");

        RefreshRecipePrice(Profiteering.Config.isHq);
        RefreshMaterialsPrice();
        IsOpen = true;
    }
    private void RefreshRecipePrice(bool isHq)
    {
        Task.Run(async () =>
        {
            string word = Dalamud.ClientState.LocalPlayer.CurrentWorld.GameData.Name.ToString();
            Response.Item item = await UniversalisClient.GetRecipePriceAsync(recipeItem.Id, word, isHq);
            Dalamud.PluginLog.Debug($"RecipeResponse:{JsonSerializer.Serialize(item)}");
            Listing listing = item.listings.FirstOrDefault();
            if (listing != null)
            {
                recipeItem.Price = listing.pricePerUnit;
            }
        });
    }

    private void RefreshMaterialsPrice()
    {
        string word = Dalamud.ClientState.LocalPlayer?.CurrentWorld.GameData?.DataCenter.Value.Name.ToString();
        int[] ids = getMaterialsId(recipeItem.Materials).ToArray();
        Dalamud.PluginLog.Debug($"ids:{String.Join(",", ids)}");
        Task.Run(async () =>
        {
            MarketDataResponse marketDataResponse = await UniversalisClient.GetMaterialsPriceAsync(ids, word);
            Dalamud.PluginLog.Debug($"MaterialsResponse:{JsonSerializer.Serialize(marketDataResponse)}");
            setMaterialsPrice(recipeItem.Materials, marketDataResponse.items);
        });
    }
    private List<int> getMaterialsId(List<RecipeItem> materials)
    {
        List<int> ids = new List<int>();
        foreach (RecipeItem material in materials)
        {
            ids.Add(material.Id);
            if (material.Materials != null)
            {
                ids.AddRange(getMaterialsId(material.Materials));
            }
        }
        return ids.Distinct().ToList();
    }
    private void setMaterialsPrice(List<RecipeItem> materials, Dictionary<int, Response.Item> items)
    {
        foreach (RecipeItem material in materials)
        {
            if (items.TryGetValue(material.Id, out Response.Item item))
            {
                int price = 0;
                foreach (var Listing in item.listings)
                {
                    price += Listing.pricePerUnit;
                }
                price /= 10;
                if (material.Price == 0 || material.Price > price)
                {
                    material.Price = price;
                    material.WorldName = item.listings.FirstOrDefault().worldName;
                }
                if (material.Materials != null)
                {
                    setMaterialsPrice(material.Materials, items);
                }
            }
        }
    }
    private void setMaterialsPrice(List<RecipeItem> materials, int id, int price)
    {
        foreach (RecipeItem material in materials)
        {
            if (material.Id == id)
            {
                material.Price = price;
            }
            if (material.Materials != null)
            {
                setMaterialsPrice(material.Materials, id, price);
            }
        }
    }
=======

    internal void Profiteering(Recipe recipe)
    {
        PVM.Profiteering(recipe);
        IsOpen = true;
    }
>>>>>>> 17e02ca (api8)
}