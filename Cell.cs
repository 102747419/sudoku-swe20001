using SplashKitSDK;
using System;

namespace sudoku_swe20001
{
    public class Cell
    {
        // Sizes
        public static readonly int FontSize = 40;
        public static readonly double Padding = 20;
        public static readonly double Width = Math.Ceiling(FontSize + Padding);

        // Fonts
        public static readonly string DefaultFont = "Roboto Bold";
        public static readonly string UserFont = "Roboto Regular";

        // Colors
        public static readonly Color DefaultNumColor = Color.Black;
        public static readonly Color UserNumColor = Color.RoyalBlue;
        public static readonly Color SelectedNumColor = Color.White;
        public static readonly Color SelectedColor = Color.LightCoral;
        public static readonly Color HoverColor = Color.LightGray;

        // Properties
        public readonly int X;
        public readonly int Y;
        public int Value;

        public Cell(int x, int y, int value = 0)
        {
            X = x;
            Y = y;
            Value = value;
        }

        /// <summary>
        /// Checks whether the cell has a value
        /// </summary>
        /// <returns></returns>
        public bool HasValue()
        {
            return Value > 0 && Value <= Grid.Size;
        }
    }
}