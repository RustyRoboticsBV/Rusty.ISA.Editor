using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISA.Editor.Scripts.UI.Graph
{
    internal static class PanelFactory
    {
        /// <summary>
        /// Create a flat style box.
        /// </summary>
        public static StyleBoxFlat GetStyleBox(Color bgColor,
            int cornerRadiusTopLeft, int cornerRadiusTopRight, int cornerRadiusBottomLeft, int cornerRadiusBottomRight,
            Color borderColor, int borderWidthTop, int borderWidthLeft, int borderWidthRight, int borderWidthBottom)
        {
            StyleBoxFlat styleBox = new();
            styleBox.BgColor = bgColor;
            styleBox.CornerRadiusTopLeft = cornerRadiusTopLeft;
            styleBox.CornerRadiusTopRight = cornerRadiusTopRight;
            styleBox.CornerRadiusBottomLeft = cornerRadiusBottomLeft;
            styleBox.CornerRadiusBottomRight = cornerRadiusBottomRight;
            styleBox.BorderColor = borderColor;
            styleBox.BorderWidthTop = borderWidthTop;
            styleBox.BorderWidthLeft = borderWidthLeft;
            styleBox.BorderWidthRight = borderWidthRight;
            styleBox.BorderWidthBottom = borderWidthBottom;
            return styleBox;
        }
    }
}
