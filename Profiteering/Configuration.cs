using Dalamud.Configuration;
using System;

namespace Profiteering;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 1;
    internal bool isBasicsMaterials;
    internal bool isHq;
    internal void Save()
    {
        Dalamud.PluginInterface.SavePluginConfig(this);
    }
    internal static Configuration Load()
    {
        if (Dalamud.PluginInterface.GetPluginConfig() is Configuration config)
        {
            return config;
        }
        config = new Configuration();
        config.Save();
        return config;
    }
}
