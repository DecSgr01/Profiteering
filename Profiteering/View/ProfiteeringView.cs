using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;
using Profiteering.ViewModel;
using System;
using System.Linq;
using System.Numerics;

namespace Profiteering.View;

internal class ProfiteeringView : Window
{
    private ImGuiSortDirection SortDirectionr = 0;
    private readonly ProfiteeringViewModel PVM;
    internal ProfiteeringView(string name, ProfiteeringViewModel profiteeringViewModel) : base(name, ImGuiWindowFlags.NoScrollbar)
    {
        PVM = profiteeringViewModel;
        Size = new(400, 220);
        SizeCondition = ImGuiCond.FirstUseEver;
    }

    public override void Draw()
    {
        ImGui.BeginChild("", ImGui.GetContentRegionAvail() with { Y = ImGui.GetContentRegionAvail().Y - ImGuiHelpers.GetButtonSize("关闭").Y - 6 });

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
                ImGuiSortDirection.Ascending => PVM.TableRows.OrderBy(x => x.Id),
                ImGuiSortDirection.Descending => PVM.TableRows.OrderByDescending(x => x.Id),
                _ => PVM.TableRows.OrderByDescending(x => x.Id)
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
                    PVM.SetMaterialsPrice(PVM.RecipeItem.Materials, item.Id, price);
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
        double grossProfit = operatingRevenue - PVM.TableRows.Sum(x => x.Total);
        double grossProfitMargin = grossProfit / operatingRevenue;
        double netProfit = operatingRevenue * 0.95 - PVM.TableRows.Sum(x => x.Total);
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

    internal void Profiteering(Recipe recipe)
    {
        PVM.Profiteering(recipe);
        IsOpen = true;
    }
}