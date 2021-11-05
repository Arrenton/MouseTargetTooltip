using Dalamud.Game.ClientState.Objects.SubKinds;
using ImGuiNET;
using System.Numerics;

namespace MouseTargetTooltip.WindowTypes
{
    public static class PlayerWindow
    {
        public static void Draw(PlayerCharacter? playerCharacter, float alpha, Configuration config)
        {
            if (playerCharacter == null) return;

            ImGui.SetNextWindowSize(new Vector2(1, 1), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSizeConstraints(new Vector2(160, 20), new Vector2(320, 320));
            float prevAlpha = ImGui.GetStyle().Alpha;
            ImGui.GetStyle().Alpha = alpha;
            if (ImGui.Begin("Tooltip Window##MouseTarget", ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoNav |
                                              ImGuiWindowFlags.NoMouseInputs
                                              | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoCollapse |
                                              ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoFocusOnAppearing))
            {
                if (config.ShowName) ImGui.Text(playerCharacter.Name.TextValue);
                if (config.ShowLevel || config.ShowJob)
                {
                    var secondLine = (config.ShowLevel ? $"Lv{playerCharacter.Level} " : "") + $"{(config.ShowJob ? Jobs.GetJobName(playerCharacter.ClassJob.Id) : "")}";
                    ImGui.Text(secondLine);
                }

                if (config.ShowHealthBar)
                    DrawMethods.DrawHealthBar(playerCharacter.CurrentHp, playerCharacter.MaxHp,
                        new Vector4(0, 0, ImGui.GetWindowWidth() - 16, 14), new Vector4(0, 0.6f, 0, 1),
                        config.ShowHpValue);

                if (config.ShowMpBar)
                {
                    var resourcePoints = 0u;
                    var maxResourcePoints = 0u;
                    if (playerCharacter.MaxMp > 0)
                    {
                        resourcePoints = playerCharacter.CurrentMp;
                        maxResourcePoints = playerCharacter.MaxMp;
                    }
                    else if (playerCharacter.MaxCp > 0)
                    {
                        resourcePoints = playerCharacter.CurrentCp;
                        maxResourcePoints = playerCharacter.MaxCp;
                    }
                    else if (playerCharacter.MaxGp > 0)
                    {
                        resourcePoints = playerCharacter.CurrentGp;
                        maxResourcePoints = playerCharacter.MaxGp;
                    }

                    DrawMethods.DrawHealthBar(resourcePoints, maxResourcePoints,
                        new Vector4(0, 0, ImGui.GetWindowWidth() - 16, 14), new Vector4(0, 0.2f, 0.6f, 1),
                        config.ShowMpValue);
                }
                ImGui.End();
            }

            ImGui.GetStyle().Alpha = prevAlpha;
        }
    }
}
