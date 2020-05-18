using System.Text;

namespace sudoku_swe20001
{
    public class Sudoku : Grid
    {
        private PuzzleData _puzzleData;
        private Cell _selected;

        public Sudoku(PuzzleData puzzleData)
        {
            _puzzleData = puzzleData;
            Reset();
        }

        /// <summary>
        /// Checks whether the puzzle is solved
        /// </summary>
        /// <returns></returns>
        public bool IsSolved()
        {
            return _puzzleData == null || GetCurrentState() == _puzzleData.Solution;
        }

        /// <summary>
        /// Resets the puzzle to its original state
        /// </summary>
        public new void Reset()
        {
            base.Reset();
            DeselectCell();

            if (_puzzleData != null)
            {
                for (int i = 0; i < _puzzleData.Puzzle.Length; i++)
                {
                    char c = _puzzleData.Puzzle[i];
                    int value = int.Parse(c.ToString());

                    int x = i % 9;
                    int y = i / 9;

                    SetValue(x, y, value);
                }
            }
        }

        /// <summary>
        /// Selects a cell
        /// </summary>
        /// <param name="cell">Cell to select</param>
        public void SelectCell(Cell cell)
        {
            _selected = cell;
        }

        /// <summary>
        /// Selects a cell
        /// </summary>
        /// <param name="x">x index of cell</param>
        /// <param name="y">y index of cell</param>
        public void SelectCell(int x, int y)
        {
            Cell cell = GetCell(x, y);
            SelectCell(cell);
        }

        /// <summary>
        /// Deselects a cell if one was selected
        /// </summary>
        public void DeselectCell()
        {
            _selected = null;
        }

        /// <summary>
        /// Checks if any cell is selected
        /// </summary>
        /// <returns></returns>
        public bool IsSelected()
        {
            return _selected != null;
        }

        /// <summary>
        /// Checks if a cell is selected
        /// </summary>
        /// <param name="cell">Cell to check</param>
        /// <returns></returns>
        public bool IsSelected(Cell cell)
        {
            return IsSelected() && _selected == cell;
        }

        /// <summary>
        /// Checks if a cell is selected
        /// </summary>
        /// <param name="x">x index of cell</param>
        /// <param name="y">x index of cell</param>
        /// <returns></returns>
        public bool IsSelected(int x, int y)
        {
            Cell cell = GetCell(x, y);
            return IsSelected(cell);
        }

        /// <summary>
        /// Checks whether the a cell's value was initially provided
        /// </summary>
        /// <param name="x">x index of cell</param>
        /// <param name="y">y index of cell</param>
        /// <returns></returns>
        public bool IsDefaultCell(int x, int y)
        {
            int index = GetIndex(x, y);
            char c = _puzzleData.Puzzle[index];
            return c != '0';
        }

        /// <summary>
        /// Checks whether the a cell's value was initially provided
        /// </summary>
        /// <param name="cell">Cell to check</param>
        /// <returns></returns>
        public bool IsDefaultCell(Cell cell)
        {
            return IsDefaultCell(cell.X, cell.Y);
        }

        /// <summary>
        /// Gets the current puzzle state
        /// </summary>
        /// <returns>String with each character being the value of each cell</returns>
        private string GetCurrentState()
        {
            var text = new StringBuilder();
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    int value = GetCell(x, y).Value;
                    text.Append(value);
                }
            }
            return text.ToString();
        }

        /// <summary>
        /// Converts coordinates to an 1-dimensional index value
        /// </summary>
        /// <param name="x">x index of cell</param>
        /// <param name="y">y index of cell</param>
        /// <returns></returns>
        private int GetIndex(int x, int y)
        {
            return x + y * 9;
        }
    }
}