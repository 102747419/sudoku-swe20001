using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Text;

namespace sudoku_swe20001
{
    public class Sudoku : Grid
    {
        // Lines
        public static readonly int ThickLineThickness = 4;
        public static readonly int ThinLineThickness = 1;
        public static readonly Color LineColor = Color.Black;

        // Properties
        private PuzzleData _puzzleData;
        private Cell _selected;
        private Point2D _position;
        private bool _solved;

        public Sudoku(PuzzleData puzzleData, Point2D position)
        {
            _position = position;
            SetPuzzle(puzzleData);
        }

        /// <summary>
        /// Updates the sudoku board
        /// </summary>
        public void Update()
        {
            // Select cell using mouse
            if (SplashKit.MouseClicked(MouseButton.LeftButton) && !IsSolved())
            {
                for (int x = 0; x < 9; x++)
                {
                    for (int y = 0; y < 9; y++)
                    {
                        Cell cell = GetCell(x, y);
                        if (CellContainsPoint(cell, SplashKit.MousePosition()))
                        {
                            SelectCell(x, y);
                        }
                    }
                }
            }

            // Reveal clue
            if (SplashKit.KeyTyped(KeyCode.CKey))
            {
                RevealClue();
            }

            // Reset puzzle
            if (SplashKit.KeyTyped(KeyCode.RKey))
            {
                Reset();
            }

            // Random puzzle
            if (SplashKit.KeyTyped(KeyCode.NKey))
            {
                SetPuzzle(Puzzles.GetRandomPuzzle());
            }

            if (IsSelected())
            {
                if (!IsDefaultCell(_selected))
                {
                    // Enter value using number keys
                    for (int i = 0; i < 9; i++)
                    {
                        if (SplashKit.KeyTyped(KeyCode.Num1Key + i) && !IsSolved())
                        {
                            _selected.Value = i + 1;
                        }
                    }

                    // Remove value
                    if (SplashKit.KeyTyped(KeyCode.BackspaceKey) && !IsSolved())
                    {
                        _selected.Value = 0;
                    }
                }

                // Move selected cell left
                if (SplashKit.KeyTyped(KeyCode.LeftKey))
                {
                    int x = (9 + _selected.X - 1) % 9;
                    SelectCell(x, _selected.Y);
                }

                // Move selected cell right
                if (SplashKit.KeyTyped(KeyCode.RightKey))
                {
                    int x = (_selected.X + 1) % 9;
                    SelectCell(x, _selected.Y);
                }

                // Move selected cell up
                if (SplashKit.KeyTyped(KeyCode.UpKey))
                {
                    int y = (9 + _selected.Y - 1) % 9;
                    SelectCell(_selected.X, y);
                }

                // Move selected cell down
                if (SplashKit.KeyTyped(KeyCode.DownKey))
                {
                    int y = (_selected.Y + 1) % 9;
                    SelectCell(_selected.X, y);
                }
            }

            if (IsSolved() && !_solved)
            {
                Console.WriteLine("Sudoku solved! Press N for a new puzzle.");
                _solved = true;
                DeselectCell();
            }
        }

        /// <summary>
        /// Draws the sudoku board to the window
        /// </summary>
        /// <param name="window">The window to draw the board onto</param>
        public void Draw(Window window)
        {
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    Cell cell = GetCell(x, y);
                    string text = cell.Value.ToString();

                    // Determine font
                    string font = IsDefaultCell(x, y) ? Cell.DefaultFont : Cell.UserFont;

                    // Get text dimensions
                    int textW = SplashKit.TextWidth(text, font, Cell.FontSize);
                    int textH = SplashKit.TextHeight(text, font, Cell.FontSize);

                    Point2D p = GetCellScreenPosition(cell);
                    double w = Cell.Width;

                    // Highlight selected cell
                    if (IsSelected(cell))
                    {
                        SplashKit.FillRectangle(Cell.SelectedColor, p.X, p.Y, w, w);
                    }
                    else if (CellContainsPoint(cell, SplashKit.MousePosition()) && !IsSolved())
                    {
                        SplashKit.FillRectangle(Cell.HoverColor, p.X, p.Y, w, w);
                    }

                    // Draw cell value
                    if (cell.HasValue())
                    {
                        p.X += (w / 2) - (textW / 2);
                        p.Y += (w / 2) - (textH / 2);

                        Color color;
                        if (IsSelected(cell))
                        {
                            color = Cell.SelectedNumColor;
                        }
                        else
                        {
                            if (IsDefaultCell(cell))
                            {
                                color = Cell.DefaultNumColor;
                            }
                            else
                            {
                                color = Cell.UserNumColor;
                            }
                        }

                        SplashKit.DrawText(text, color, font, Cell.FontSize, p.X, p.Y);
                    }
                }
            }

            DrawingOptions thinLineOpts = new DrawingOptions
            {
                Dest = window,
                LineWidth = ThinLineThickness
            };

            DrawingOptions thickLineOpts = new DrawingOptions
            {
                Dest = window,
                LineWidth = ThickLineThickness
            };

            // Draw grid lines
            for (int i = 0; i <= 9; i++)
            {
                double offset = (i - 4.5) * Cell.Width;
                double extents = ThickLineThickness / 2;

                // Determine line thickness
                DrawingOptions opts = (i % 3 == 0) ? thickLineOpts : thinLineOpts;

                // Draw vertical line
                SplashKit.DrawLine(
                    LineColor,
                    _position.X + offset,
                    _position.Y - 4.5 * Cell.Width - extents,
                    _position.X + offset,
                    _position.Y + 4.5 * Cell.Width + extents,
                    opts
                );

                // Draw horizontal line
                SplashKit.DrawLine(
                    LineColor,
                    _position.X - 4.5 * Cell.Width - extents,
                    _position.Y + offset,
                    _position.X + 4.5 * Cell.Width + extents,
                    _position.Y + offset,
                    opts
                );
            }
        }

        /// <summary>
        /// Reveals one clue. Mistakes are corrected before empty cells are filled.
        /// </summary>
        public void RevealClue()
        {
            if (!IsSolved())
            {
                // Initialize variables
                Random r = new Random();
                Cell cell;
                int[] pt;
                int x;
                int y;
                int index;

                // Initialize lists
                List<int[]> missing = new List<int[]>();
                List<int[]> incorrect = new List<int[]>();

                // Collect cells with missing or incorrect values
                for (y = 0; y < 9; y++)
                {
                    for (x = 0; x < 9; x++)
                    {
                        cell = GetCell(x, y);
                        pt = new int[] { x, y };

                        if (!cell.HasValue())
                        {
                            missing.Add(pt);
                        }
                        else if (!CellHasCorrectValue(cell))
                        {
                            incorrect.Add(pt);
                        }
                    }
                }

                // Select random cell with incorrect or missing value, prioritising incorrect cells
                if (incorrect.Count > 0)
                {
                    index = r.Next(incorrect.Count);
                    pt = incorrect[index];
                }
                else
                {
                    index = r.Next(missing.Count);
                    pt = missing[index];
                }

                // Get incorrect cell
                x = pt[0];
                y = pt[1];
                cell = GetCell(x, y);

                // Get correct value from solution
                index = GetIndex(x, y);
                int value = int.Parse(_puzzleData.Solution[index].ToString());

                // Select and correct cell value
                SelectCell(x, y);
                cell.Value = value;
            }
        }

        /// <summary>
        /// Sets the sudoku to a specific puzzle
        /// </summary>
        /// <param name="puzzleData">New puzzle</param>
        public void SetPuzzle(PuzzleData puzzleData)
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
            _solved = false;
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

        /// <summary>
        /// Gets a cell's screen position based on its dimensions
        /// </summary>
        /// <param name="cell">Cell to get position of</param>
        /// <returns>Screen position</returns>
        private Point2D GetCellScreenPosition(Cell cell)
        {
            return new Point2D()
            {
                X = _position.X + (cell.X - 4.5) * (Cell.FontSize + Cell.Padding),
                Y = _position.Y + (cell.Y - 4.5) * (Cell.FontSize + Cell.Padding)
            };
        }

        /// <summary>
        /// Checks whether a point lies within a cell
        /// </summary>
        /// <param name="cell">Cell to check</param>
        /// <param name="pt">Screen position</param>
        /// <returns></returns>
        private bool CellContainsPoint(Cell cell, Point2D pt)
        {
            Point2D pos = GetCellScreenPosition(cell);

            bool left = pt.X >= pos.X;
            bool right = pt.X < pos.X + Cell.Width;
            bool top = pt.Y >= pos.Y;
            bool bottom = pt.Y < pos.Y + Cell.Width;

            return left && right && top && bottom;
        }

        /// <summary>
        /// Checks whether a cell has the correct value
        /// </summary>
        /// <param name="cell">Cell to check</param>
        /// <returns></returns>
        private bool CellHasCorrectValue(Cell cell)
        {
            int index = GetIndex(cell.X, cell.Y);
            int correctValue = int.Parse(_puzzleData.Solution[index].ToString());
            return cell.Value == correctValue;
        }
    }
}