<<<<<<< HEAD
﻿using ECommons;
using Profiteering.Manager;
=======
﻿using Profiteering.Manager;
>>>>>>> 17e02ca (api8)
using Dalamud.ContextMenu;
using Profiteering.View;
using Dalamud.Plugin;
using Dalamud.Interface.Windowing;
using Lumina.Excel.GeneratedSheets;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
<<<<<<< HEAD
=======
using Profiteering.ViewModel;
>>>>>>> 17e02ca (api8)

namespace Profiteering;
public sealed class Profiteering : IDalamudPlugin
{
<<<<<<< HEAD
    public string InternalName { get; } = "Profiteering";
    public string Name { get; } = "Profiteering";
=======
    public static string Name => "Profiteering";
>>>>>>> 17e02ca (api8)
    public static Configuration Config { get; private set; } = null!;
    private readonly DalamudContextMenu contextMenuBase;
    private readonly ProfiteeringView profiteeringView;
    private readonly WindowSystem windowSystem;
    public Profiteering(DalamudPluginInterface Interface)
    {
<<<<<<< HEAD
        InternalName = "Profiteering";
        Dalamud.Initialize(Interface);
        ECommonsMain.Init(Interface, this);

        Config = Configuration.Load();

        profiteeringView = new ProfiteeringView();
=======
        Dalamud.Initialize(Interface);
        Config = Configuration.Load();
        profiteeringView = new ProfiteeringView(Name, new ProfiteeringViewModel());
>>>>>>> 17e02ca (api8)
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
<<<<<<< HEAD
        Item item = Dalamud.DataManager.GetExcelSheet<Item>()?.GetRow(i);
=======
        Item? item = Dalamud.DataManager.GetExcelSheet<Item>()!.GetRow(i);
>>>>>>> 17e02ca (api8)
        if (item == null)
        {
            return;
        }
<<<<<<< HEAD

        var recipe = RecipeManager.GetRecipebyItemId((int)item.RowId);
=======
        var recipe = RecipeManager.GetRecipebyItemId(item.RowId);
>>>>>>> 17e02ca (api8)
        if (recipe == null)
        {
            return;
        }

<<<<<<< HEAD
        args.AddCustomItem(new InventoryContextMenuItem(new SeString(new TextPayload("Profiteering")), (_) =>
        {
            profiteeringView.profiteering(recipe);
=======
        args.AddCustomItem(new InventoryContextMenuItem(new SeString(new TextPayload(Name)), _ =>
        {
            profiteeringView.Profiteering(recipe);
>>>>>>> 17e02ca (api8)
        }, true));
    }

    private void ContextMenuOnOpenGameObjectContextMenu(GameObjectContextMenuOpenArgs args)
    {
        uint i = (uint)(Dalamud.GameGui.HoveredItem % 500000);
<<<<<<< HEAD
        Item item = Dalamud.DataManager.GetExcelSheet<Item>().GetRow(i);
=======
        Item? item = Dalamud.DataManager.GetExcelSheet<Item>()!.GetRow(i);
>>>>>>> 17e02ca (api8)
        if (item == null)
        {
            return;
        }
<<<<<<< HEAD

        var recipe = RecipeManager.GetRecipebyItemId((int)item.RowId);
=======
        var recipe = RecipeManager.GetRecipebyItemId(item.RowId);
>>>>>>> 17e02ca (api8)
        if (recipe == null)
        {
            return;
        }
<<<<<<< HEAD
        args.AddCustomItem(new GameObjectContextMenuItem(new SeString(new TextPayload("Profiteering")), (_) =>
        {
            profiteeringView.profiteering(recipe);
=======
        args.AddCustomItem(new GameObjectContextMenuItem(new SeString(new TextPayload(Name)), _ =>
        {
            profiteeringView.Profiteering(recipe);
>>>>>>> 17e02ca (api8)
        }, true));
    }



    public void Dispose()
    {
<<<<<<< HEAD
        ECommonsMain.Dispose();
        Dalamud.PluginInterface.UiBuilder.Draw -= windowSystem.Draw;
        // Remove context menu handler
        contextMenuBase.OnOpenInventoryContextMenu -= this.ContextMenuOnOpenInventoryContextMenu;
        contextMenuBase.OnOpenGameObjectContextMenu -= this.ContextMenuOnOpenGameObjectContextMenu;
=======
        Dalamud.PluginInterface.UiBuilder.Draw -= windowSystem.Draw;
        // Remove context menu handler
        contextMenuBase.OnOpenInventoryContextMenu -= ContextMenuOnOpenInventoryContextMenu;
        contextMenuBase.OnOpenGameObjectContextMenu -= ContextMenuOnOpenGameObjectContextMenu;
>>>>>>> 17e02ca (api8)
        contextMenuBase.Dispose();
    }
}
