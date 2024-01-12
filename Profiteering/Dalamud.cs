using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
namespace Profiteering;
internal class Dalamud
{
    internal static void Initialize(DalamudPluginInterface pluginInterface)
        => pluginInterface.Create<Dalamud>();

    // @formatter:off
    [PluginService][RequiredVersion("1.0")] internal static DalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService][RequiredVersion("1.0")] internal static IPluginLog PluginLog { get; private set; } = null!;
    [PluginService][RequiredVersion("1.0")] internal static IDataManager DataManager { get; private set; } = null!;
    [PluginService][RequiredVersion("1.0")] internal static IClientState ClientState { get; private set; } = null!;
    [PluginService][RequiredVersion("1.0")] internal static IGameGui GameGui { get; private set; } = null!;
    [PluginService][RequiredVersion("1.0")] internal static ITextureProvider TextureProvider { get; private set; } = null!;
    // @formatter:on
}
