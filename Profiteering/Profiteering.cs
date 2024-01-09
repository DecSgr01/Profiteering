using ECommons;
using Profiteering.Manager;
using Dalamud.ContextMenu;
using Profiteering.View;
using Dalamud.Plugin;
using Dalamud.Interface.Windowing;
using Lumina.Excel.GeneratedSheets;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;

namespace Profiteering;
public sealed class Profiteering : IDalamudPlugin
{
    public string InternalName { get; } = "Profiteering";
    public string Name { get; } = "Profiteering";
    public static Configuration Config { get; private set; } = null!;
    private readonly DalamudContextMenu contextMenuBase;
    private readonly ProfiteeringView profiteeringView;
    private readonly WindowSystem windowSystem;
    public Profiteering(DalamudPluginInterface Interface)
    {
        InternalName = "Profiteering";
        Dalamud.Initialize(Interface);
        ECommonsMain.Init(Interface, this);

        Config = Configuration.Load();

        profiteeringView = new ProfiteeringView();
        windowSystem = new WindowSystem(Name);
        windowSystem.AddWindow(profiteeringView);
        Dalamud.PluginInterface.UiBuilder.Draw += windowSystem.Draw;
        // Set up context menu
        contextMenuBase = new DalamudContextMenu(Interface);
        contextMenuBase.OnOpenInventoryContextMenu += ContextMenuOnOpenInventoryContextMenu;
        contextMenuBase.OnOpenGameObjectContextMenu += ContextMenuOnOpenGameObjectContextMenu;
    }
    private void ContextMenuOnOpenInventoryContextMenu(InventoryContextMenuOpenArgs args)
    {
        uint i = (uint)(Dalamud.GameGui.HoveredItem % 500000);
        Item item = Dalamud.DataManager.GetExcelSheet<Item>()?.GetRow(i);
        if (item == null)
        {
            return;
        }

        var recipe = RecipeManager.GetRecipebyItemId((int)item.RowId);
        if (recipe == null)
        {
            return;
        }

        args.AddCustomItem(new InventoryContextMenuItem(new SeString(new TextPayload("Profiteering")), (_) =>
        {
            profiteeringView.profiteering(recipe);
        }, true));
    }

    private void ContextMenuOnOpenGameObjectContextMenu(GameObjectContextMenuOpenArgs args)
    {
        uint i = (uint)(Dalamud.GameGui.HoveredItem % 500000);
        Item item = Dalamud.DataManager.GetExcelSheet<Item>().GetRow(i);
        if (item == null)
        {
            return;
        }

        var recipe = RecipeManager.GetRecipebyItemId((int)item.RowId);
        if (recipe == null)
        {
            return;
        }
        args.AddCustomItem(new GameObjectContextMenuItem(new SeString(new TextPayload("Profiteering")), (_) =>
        {
            profiteeringView.profiteering(recipe);
        }, true));
    }



    public void Dispose()
    {
        ECommonsMain.Dispose();
        Dalamud.PluginInterface.UiBuilder.Draw -= windowSystem.Draw;
        // Remove context menu handler
        contextMenuBase.OnOpenInventoryContextMenu -= this.ContextMenuOnOpenInventoryContextMenu;
        contextMenuBase.OnOpenGameObjectContextMenu -= this.ContextMenuOnOpenGameObjectContextMenu;
        contextMenuBase.Dispose();
    }
}
