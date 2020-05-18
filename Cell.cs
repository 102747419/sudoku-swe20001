namespace sudoku_swe20001
{
    public class Cell
    {
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