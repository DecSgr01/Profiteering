using Dalamud.Game;
using Dalamud.Game.ClientState.Objects;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
namespace Profiteering;
public class Dalamud
{
    public static void Initialize(DalamudPluginInterface pluginInterface)
        => pluginInterface.Create<Dalamud>();

    // @formatter:off
    [PluginService][RequiredVersion("1.0")] public static DalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService][RequiredVersion("1.0")] public static ICommandManager CommandManager { get; private set; } = null!;
    [PluginService][RequiredVersion("1.0")] public static IPluginLog PluginLog { get; private set; } = null!;
    [PluginService][RequiredVersion("1.0")] public static ISigScanner SigScanner { get; private set; } = null!;
    [PluginService][RequiredVersion("1.0")] public static IDataManager DataManager { get; private set; } = null!;
    [PluginService][RequiredVersion("1.0")] public static IClientState ClientState { get; private set; } = null!;
    [PluginService][RequiredVersion("1.0")] public static IChatGui ChatGui { get; private set; } = null!;
    [PluginService][RequiredVersion("1.0")] public static IFramework Framework { get; private set; } = null!;
    [PluginService][RequiredVersion("1.0")] public static ICondition Condition { get; private set; } = null!;
    [PluginService][RequiredVersion("1.0")] public static IKeyState KeyState { get; private set; } = null!;
    [PluginService][RequiredVersion("1.0")] public static IGameGui GameGui { get; private set; } = null!;
    [PluginService][RequiredVersion("1.0")] public static ITargetManager TargetManager { get; private set; } = null!;
    [PluginService][RequiredVersion("1.0")] public static IGameInteropProvider GameInteropProvider { get; private set; } = null!;
    [PluginService][RequiredVersion("1.0")] public static ITextureProvider TextureProvider { get; private set; } = null!;
    // @formatter:on
}