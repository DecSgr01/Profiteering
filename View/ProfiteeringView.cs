using Dalamud.Interface;
using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using ECommons.DalamudServices;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;
using Profiteering.Client;
using Profiteering.DTO;
using Profiteering.Manager;
using Profiteering.Response;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Profiteering.View;

internal class ProfiteeringView : Window
{
    private ImGuiSortDirection SortDirectionr = 0;
    RecipeItemDTO recipeItemDTO;
    Recipe recipe;
    int count;

    public ProfiteeringView() : base("AutoCrafterView", ImGuiWindowFlags.NoScrollbar)
    {
        Vector2 minSize = new Vector2(400, 220);
        this.Size = minSize;
        this.SizeCondition = ImGuiCond.FirstUseEver;
    }

    public override void Draw()
    {
        ImGui.BeginChild("", ImGui.GetContentRegionAvail() with { Y = ImGui.GetContentRegionAvail().Y - ImGuiHelpers.GetButtonSize("关闭").Y - 6 });

        if (ImGui.Checkbox("基础素材", ref Profiteering.Instance.config.isBasicsMaterials))
        {
            Profiteering.Instance.saveConfig();
            RefreshMaterials(recipe);
            RefreshMarketData();
        }
        if (ImGui.InputInt(":个数", ref count, recipeItemDTO.count))
        {
            if (count <= 0) count = recipeItemDTO.count;
        }

        int num = count / recipeItemDTO.count;
        if (count % recipeItemDTO.count > 0)
        {
            num++;
        }
        ImGuiTableFlags flags = ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.RowBg | ImGuiTableFlags.Borders | ImGuiTableFlags.Resizable | ImGuiTableFlags.Reorderable | ImGuiTableFlags.Hideable | ImGuiTableFlags.Sortable;
        if (ImGui.BeginTable("table1", 5, flags))
        {
            ImGui.TableSetupColumn("id", ImGuiTableColumnFlags.DefaultHide | ImGuiTableColumnFlags.WidthFixed);
            ImGui.TableSetupColumn("名称", ImGuiTableColumnFlags.WidthFixed);
            ImGui.TableSetupColumn("数量", ImGuiTableColumnFlags.WidthFixed);
            ImGui.TableSetupColumn("平均单价", ImGuiTableColumnFlags.WidthFixed);
            ImGui.TableSetupColumn("总价", ImGuiTableColumnFlags.WidthStretch);
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
                ImGuiSortDirection.Ascending => recipeItemDTO?.Materials.OrderBy(x => x.id),
                ImGuiSortDirection.Descending => recipeItemDTO?.Materials.OrderByDescending(x => x.id),
                _ => recipeItemDTO?.Materials.OrderByDescending(x => x.id)
            };

            float? unitPrice = 0;
            float? Totalprice = 0;

            foreach (var item in items)
            {
                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);
                ImGui.Text($"{item?.id}");
                ImGui.TableSetColumnIndex(1);
                ImGui.Text($"{item?.name}");
                ImGui.TableSetColumnIndex(2);
                ImGui.Text($"{item?.count * num}");
                ImGui.TableSetColumnIndex(3);
                unitPrice += item?.averageUnitPrice;
                ImGui.Text($"{item?.averageUnitPrice}");
                ImGui.TableSetColumnIndex(4);
                Totalprice += item?.averageUnitPrice * item?.count * num;
                ImGui.Text($"{string.Format("{0:f2}", item?.averageUnitPrice * item?.count * num)}");
            }
            ImGui.TableNextRow();
            ImGui.TableSetColumnIndex(1);
            ImGui.Text("总计");
            ImGui.TableSetColumnIndex(3);
            ImGui.Text($"{string.Format("{0:f2}", unitPrice)}");
            ImGui.TableSetColumnIndex(4);
            ImGui.Text($"{string.Format("{0:f2}", Totalprice)}");
            ImGui.EndTable();
        }
        ImGui.EndChild();
        ImGui.Separator();
        ImGui.NewLine();
        ImGui.SameLine(ImGui.GetContentRegionAvail().X - ImGuiHelpers.GetButtonSize("关闭").X - 6);

        if (ImGui.Button("关闭"))
        {
            IsOpen = false;
            PluginLog.Log("Settings saved.");
        }

        ImGui.End();
    }

    public void profiteering(Recipe recipe)
    {
        this.recipe = recipe;
        this.IsOpen = true;
        List<MaterialDTO> Materials;
        if (Profiteering.Instance.config.isBasicsMaterials)
        {
            Materials = MaterialManager.getBasicsMaterials(recipe);
        }
        else
        {
            Materials = MaterialManager.getDirectMaterials(recipe);
        }
        recipeItemDTO = new RecipeItemDTO(recipe.ItemResult.Row, recipe.ItemResult.Value?.Name.ToString(), recipe.AmountResult, Materials);
        count = recipe.AmountResult;
        // PluginLog.Log($"Materials:{string.Join(",", Materials)}");
        RefreshMarketData();
    }


    private void RefreshMaterials(Recipe recipe)
    {
        if (recipe != null && recipeItemDTO != null)
        {
            List<MaterialDTO> Materials;
            if (Profiteering.Instance.config.isBasicsMaterials)
            {
                Materials = MaterialManager.getBasicsMaterials(recipe);
            }
            else
            {
                Materials = MaterialManager.getDirectMaterials(recipe);
            }
            recipeItemDTO.Materials = Materials;
            // PluginLog.Log($"Materials:{string.Join(",", Materials)}");
        }

    }

    private void RefreshMarketData()
    {
        Task.Run(async () =>
        {
            if (recipeItemDTO != null)
            {
                List<MaterialDTO> Materials = recipeItemDTO.Materials;
                int[] ints = Materials.Select(x => x.id).ToArray();
                string word = Svc.ClientState.LocalPlayer?.CurrentWorld.GameData?.DataCenter.Value?.Name.ToString();
                MarketDataResponse marketDataResponse = await UniversalisClient.GetCurrentDataAsync(ints, word, CancellationToken.None);
                string v = JsonSerializer.Serialize(marketDataResponse);
                foreach (var Material in Materials)
                {
                    item item = marketDataResponse?.items[Material.id];
                    foreach (var Listing in item.listings)
                    {
                        Material.averageUnitPrice += Listing.pricePerUnit;
                    }
                    Material.averageUnitPrice /= 10;
                }
            }
        });
    }
}