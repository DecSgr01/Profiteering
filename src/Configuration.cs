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
}
