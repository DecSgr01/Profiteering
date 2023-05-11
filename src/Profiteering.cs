using Dalamud.Plugin;
using ECommons;
using ECommons.DalamudServices;
using Lumina.Excel.GeneratedSheets;
using Profiteering.Manager;
using Dalamud.ContextMenu;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Profiteering.View;
using Dalamud.Interface.Windowing;

namespace Profiteering;
public sealed class Profiteering : IDalamudPlugin
{
    public string Name => "Profiteering";
    internal static Profiteering Instance { get; private set; } = null!;
    internal Configuration config { get; }
    private readonly DalamudContextMenu contextMenuBase;
    private readonly ProfiteeringView profiteeringView;
    private readonly WindowSystem windowSystem;
    public Profiteering(DalamudPluginInterface pluginInterface)

    {
        Instance = this;
        ECommonsMain.Init(pluginInterface, this);
        config = Svc.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

        profiteeringView = new ProfiteeringView();
        windowSystem = new WindowSystem(Name);
        windowSystem.AddWindow(profiteeringView);

        Svc.PluginInterface.UiBuilder.Draw += windowSystem.Draw;
        // Set up context menu
        this.contextMenuBase = new DalamudContextMenu();
        this.contextMenuBase.OnOpenInventoryContextMenu += this.ContextMenuOnOpenInventoryContextMenu;
        this.contextMenuBase.OnOpenGameObjectContextMenu += this.ContextMenuOnOpenGameObjectContextMenu;
    }
    private void ContextMenuOnOpenInventoryContextMenu(InventoryContextMenuOpenArgs args)
    {
        uint i = (uint)(Svc.GameGui.HoveredItem % 500000);
        Item item = Svc.Data.GetExcelSheet<Item>()?.GetRow(i);
        if (item == null)
        {
            return;
        }

        var recipe = RecipeManager.getRecipebyItemId((int)item.RowId);
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
        uint i = (uint)(Svc.GameGui.HoveredItem % 500000);
        Item item = Svc.Data.GetExcelSheet<Item>().GetRow(i);
        if (item == null)
        {
            return;
        }

        var recipe = RecipeManager.getRecipebyItemId((int)item.RowId);
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
        Svc.PluginInterface.UiBuilder.Draw -= windowSystem.Draw;
        // Remove context menu handler
        this.contextMenuBase.OnOpenInventoryContextMenu -= this.ContextMenuOnOpenInventoryContextMenu;
        this.contextMenuBase.OnOpenGameObjectContextMenu -= this.ContextMenuOnOpenGameObjectContextMenu;
        this.contextMenuBase.Dispose();
    }

    internal void saveConfig()
    {
        Svc.PluginInterface.SavePluginConfig(config);
    }
}
