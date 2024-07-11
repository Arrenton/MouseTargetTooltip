using ImGuiNET;
using MouseTargetTooltip.Enums;
using System.Numerics;
using static MouseTargetTooltip.Constants.Constants;

namespace MouseTargetTooltip.WindowTypes
{
    public static class DebugWindow
    {
        public static void Draw(float alpha, Configuration config)
        {
            ImGui.SetNextWindowSize(new Vector2(1, 1), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSizeConstraints(new Vector2(160, 20), new Vector2(320, 320));
            float prevAlpha = ImGui.GetStyle().Alpha;
            ImGui.GetStyle().Alpha = alpha;
            if (ImGui.Begin("Tooltip Window##MouseTarget",
                ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoCollapse |
                ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoFocusOnAppearing))
            {
                ImGui.Text("NAME");
                if (config.ShowLevel || config.ShowJob)
                {
                    var secondLine = (config.ShowLevel ? $"Lv?? " : "") + $"{(config.ShowJob ? "JOB" : "")}";
                    ImGui.Text(secondLine);
                }

                if (config.ShowHealthBar)
                    DrawMethods.DrawHealthBar(999, 999, new Vector4(0, 0, ImGui.GetWindowWidth() - 16, 14),
                        new Vector4(0, 0.6f, 0, 1), config.ShowHpValue);
                if (config.ShowMpBar)
                    DrawMethods.DrawHealthBar(999, 999, new Vector4(0, 0, ImGui.GetWindowWidth() - 16, 14),
                        new Vector4(0, 0.2f, 0.6f, 1), config.ShowMpValue);

                
                if (ImGui.IsMouseDragging(ImGuiMouseButton.Left))
                {
                    var pos = ImGui.GetWindowPos();
                    var size = ImGui.GetWindowSize();
                    MouseTooltipPlugin.Ui!.Configuration.TooltipX = (int)(pos.X + size.X * PivotAlignment(WindowAlignment.TopLeft).X);
                    MouseTooltipPlugin.Ui.Configuration.TooltipY = (int)(pos.Y + size.Y * PivotAlignment(WindowAlignment.TopLeft).Y);
                    MouseTooltipPlugin.Ui!.Configuration.Save();
                }
                ImGui.End();
            }

            ImGui.GetStyle().Alpha = prevAlpha;
        }
    }
}
