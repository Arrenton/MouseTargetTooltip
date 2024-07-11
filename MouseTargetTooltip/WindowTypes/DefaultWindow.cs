using ImGuiNET;
using System.Numerics;
using Dalamud.Game.ClientState.Objects.Types;

namespace MouseTargetTooltip.WindowTypes
{
    public static class DefaultWindow
    {
        public static void Draw(IGameObject gameObject, float alpha)
        {
            ImGui.SetNextWindowSize(new Vector2(1, 1), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSizeConstraints(new Vector2(40, 20), new Vector2(320, 320));
            float prevAlpha = ImGui.GetStyle().Alpha;
            ImGui.GetStyle().Alpha = alpha;
            if (ImGui.Begin("Tooltip Window##MouseTarget", ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoNav | ImGuiWindowFlags.NoMouseInputs 
                                              | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoFocusOnAppearing))
            {
                ImGui.Text(gameObject.Name.TextValue);
                ImGui.End();
            }
            ImGui.GetStyle().Alpha = prevAlpha;
        }
    }
}
