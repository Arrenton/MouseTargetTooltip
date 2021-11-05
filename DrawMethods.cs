using ImGuiNET;
using System.Numerics;

namespace MouseTargetTooltip
{
    public static class DrawMethods
    {
        public static void DrawHealthBar(uint current, uint maximum, Vector4 position, Vector4 color, bool showValue)
        {
            Vector2 size = new Vector2(position.Z, position.W);
            Vector2 innerBarSize = new Vector2(position.Z * (current / (float)maximum), position.W);
            Vector2 pos = ImGui.GetCursorScreenPos() + new Vector2(position.X, position.Y);
            Vector2 textPos = new Vector2(size.X / 2f - size.X, size.Y / 2f);

            string textValue = $"{SpecialValueText(current)} / {SpecialValueText(maximum)}";

            uint col = ImGui.GetColorU32(color);
            uint bgCol = ImGui.GetColorU32(new Vector4(0.3f, 0.3f, 0.3f, 1));

            ImGui.GetWindowDrawList().AddRectFilled(pos, pos + size, bgCol); // bg
            ImGui.GetWindowDrawList().AddRectFilled(pos, pos + innerBarSize, col); // fill

            ImGui.Dummy(size);

            if (showValue)
            {
                ImGui.SameLine(0, 0);
                TextCenteredShadowed(textValue, 1f, textPos, new Vector4(255 / 255f, 255 / 255f, 255 / 255f, 1f),
                    new Vector4(0 / 255f, 0 / 255f, 0 / 255f, 0.25f), 3);
                ImGui.NewLine();
            }
        }

        public static void TextShadowedDrawList(ImDrawListPtr drawList, string text, Vector2 position, Vector4 foregroundColor, Vector4 shadowColor, byte shadowWidth = 1)
        {
            var x = position.X;
            var y = position.Y;

            for (var i = -shadowWidth; i < shadowWidth; i++)
            {
                for (var j = -shadowWidth; j < shadowWidth; j++)
                {
                    if (i == 0 && j == 0) continue;
                    drawList.AddText(new Vector2(x + i, y + j), ImGui.GetColorU32(shadowColor), text);
                }
            }
            drawList.AddText(new Vector2(x, y), ImGui.GetColorU32(foregroundColor), text);
        }

        public static void TextCenteredShadowed(string text, float size, Vector2 position, Vector4 foregroundColor, Vector4 shadowColor, byte shadowWidth = 1)
        {
            ImGui.SetWindowFontScale(size);
            var textSize = ImGui.CalcTextSize(text);
            var prevX = ImGui.GetCursorPosX();
            var prevY = ImGui.GetCursorPosY();
            var x = prevX + position.X - textSize.X / 2;
            var y = prevY + position.Y - textSize.Y / 2;

            for (var i = -shadowWidth; i < shadowWidth; i++)
            {
                for (var j = -shadowWidth; j < shadowWidth; j++)
                {
                    if (i == 0 && j == 0) continue;
                    ImGui.SetCursorPosX(x + i);
                    ImGui.SetCursorPosY(y + j);
                    ImGui.TextColored(shadowColor, text);
                }
            }
            ImGui.SetCursorPosX(x);
            ImGui.SetCursorPosY(y);
            ImGui.TextColored(foregroundColor, text);
            ImGui.SetCursorPosX(prevX);
            ImGui.SetCursorPosY(prevY);
            ImGui.SetWindowFontScale(1);
        }

        public static string SpecialValueText(uint value)
        {
            if (value > 1000000)
                return $"{value / 1000000f:0.#}M";

            if (value > 100000)
                return $"{value / 1000f:0}K";

            if (value > 10000)
                return $"{value / 1000f:0.#}K";

            return value.ToString();
        }
    }
}
