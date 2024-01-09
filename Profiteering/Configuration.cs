using Dalamud.Configuration;
using System;

namespace Profiteering;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 1;
    public bool isBasicsMaterials;
    public bool isHq;
    public int num;
    public void Save()
    {
        Dalamud.PluginInterface.SavePluginConfig(this);
    }

    public static Configuration Load()
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
