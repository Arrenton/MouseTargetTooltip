using MouseTargetTooltip.Enums;
using System.Numerics;

namespace MouseTargetTooltip.Constants
{
    public static class Constants
    {
        public static Vector2 PivotAlignment(Enums.WindowAlignment alignment)
        {
            switch (alignment)
            {
                case WindowAlignment.Center:
                    return new Vector2(0.5f, 0.5f);
                case WindowAlignment.TopRight:
                    return new Vector2(1, 0);
                case WindowAlignment.BottomLeft:
                    return new Vector2(0, 1);
                case WindowAlignment.BottomRight:
                    return new Vector2(1, 1);
                case WindowAlignment.Top:
                    return new Vector2(0.5f, 0);
                case WindowAlignment.Left:
                    return new Vector2(0, 0.5f);
                case WindowAlignment.Right:
                    return new Vector2(1, 0.5f);
                case WindowAlignment.Bottom:
                    return new Vector2(0.5f, 1);
                default:
                    return new Vector2(0, 0);

            }
        }
    }
}
