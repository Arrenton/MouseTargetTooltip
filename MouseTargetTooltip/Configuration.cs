using Dalamud.Configuration;
using Dalamud.Plugin;
using System;
using MouseTargetTooltip.Enums;

namespace MouseTargetTooltip
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;
        public int TooltipX { get; set; } = 300;
        public int TooltipY { get; set; } = 300;
        public WindowAlignment TooltipAlignment { get; set; } = WindowAlignment.TopLeft;

        public bool Movable { get; set; } = true;

        public bool ShowHealthBar { get; set; } = true;

        public bool ShowMpBar { get; set; } = true;

        public bool ShowHpValue { get; set; } = true;

        public bool ShowMpValue { get; set; } = true;

        public bool ShowLevel { get; set; } = true;

        public bool ShowJob { get; set; } = true;

        public bool ShowName { get; set; } = true;
        public float FadeDelay { get; set; } = 1.25f;
        public float FadeTime { get; set; } = 0.5f;

        // the below exist just to make saving less cumbersome

        [NonSerialized]
        private DalamudPluginInterface? _pluginInterface;

        public void Initialize(DalamudPluginInterface? pluginInterface)
        {
            this._pluginInterface = pluginInterface;
        }

        public void Save()
        {
            this._pluginInterface?.SavePluginConfig(this);
        }
    }
}
