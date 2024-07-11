using Dalamud.Game.ClientState.Objects.Types;
using ImGuiNET;
using System.Numerics;

namespace MouseTargetTooltip.WindowTypes
{
    public static class BattleNpcWindow
    {
        public static void Draw(IBattleNpc? battleNpc, float alpha, Configuration config)
        {
            if (battleNpc == null) return;

            ImGui.SetNextWindowSize(new Vector2(1, 1), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSizeConstraints(new Vector2(160, 20), new Vector2(320, 320));
            float prevAlpha = ImGui.GetStyle().Alpha;
            ImGui.GetStyle().Alpha = alpha;
            if (ImGui.Begin("Tooltip Window##MouseTarget", ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoNav |
                                                           ImGuiWindowFlags.NoMouseInputs
                                                           | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoCollapse |
                                                           ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoFocusOnAppearing))
            {
                if (config.ShowName) ImGui.Text(battleNpc.Name.TextValue);
                if (config.ShowLevel || config.ShowJob)
                {
                    var secondLine = (config.ShowLevel ? $"Lv{battleNpc.Level} " : "") + $"{(config.ShowJob ? Jobs.GetJobName(battleNpc.ClassJob.Id) : "")}";
                    ImGui.Text(secondLine);
                }

                if (config.ShowHealthBar)
                    DrawMethods.DrawHealthBar(battleNpc.CurrentHp, battleNpc.MaxHp,
                        new Vector4(0, 0, ImGui.GetWindowWidth() - 16, 14),
                        new Vector4(0, 0.6f, 0, 1), config.ShowHpValue);
                
                ImGui.End();
            }

            ImGui.GetStyle().Alpha = prevAlpha;
        }
    }
}
