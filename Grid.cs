namespace sudoku_swe20001
{
    public class Grid
    {
        private Cell[,] _cells;

        public Grid()
        {
            Reset();
        }

        /// <summary>
        /// Gets a cell
        /// </summary>
        /// <param name="x">x index of cell</param>
        /// <param name="y">y index of cell</param>
        /// <returns>A cell</returns>
        public Cell GetCell(int x, int y)
        {
            if (IsValidCell(x, y))
            {
                return _cells[x, y];
            }
            return null;
        }

        /// <summary>
        /// Sets a cell's value
        /// </summary>
        /// <param name="x">x index of cell</param>
        /// <param name="y">y index of cell</param>
        /// <param name="value">Value to set</param>
        public void SetValue(int x, int y, int value)
        {
            Cell cell = GetCell(x, y);
            if (cell != null)
            {
                cell.Value = value;
            }
        }

        /// <summary>
        /// Resets the cells
        /// </summary>
        public void Reset()
        {
            _cells = new Cell[9, 9];
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    _cells[x, y] = new Cell(x, y);
                }
            }
        }

        /// <summary>
        /// Checks whether a grid position contains a cell
        /// </summary>
        /// <param name="x">x index of grid</param>
        /// <param name="y">y index of grid</param>
        /// <returns></returns>
        private bool IsValidCell(int x, int y)
        {
            return x >= 0 && x < 9 && y >= 0 && y < 9;
        }
    }
}