using NUnit.Framework;
using SplashKitSDK;

namespace sudoku_swe20001
{
    [TestFixture]
    public class TestSudoku
    {
        private Sudoku _sudoku;

        private readonly string _puzzle = new string('0', 81);
        private readonly string _solution = new string('1', 81);

        [SetUp]
        public void SetUp()
        {
            PuzzleData puzzleData = new PuzzleData(_puzzle, _solution);
            _sudoku = new Sudoku(puzzleData, new Point2D());
        }

        [Test]
        public void TestIsSolvedWhenPuzzleIsComplete()
        {
            string solution = new string('1', 81);
            PuzzleData puzzleData = new PuzzleData(solution, solution);
            _sudoku = new Sudoku(puzzleData, new Point2D());
            Assert.IsTrue(_sudoku.IsSolved());
        }

        [Test]
        public void TestIsSolvedWhenNoPuzzle()
        {
            _sudoku = new Sudoku(null, new Point2D());
            Assert.IsTrue(_sudoku.IsSolved());
        }

        [Test]
        public void TestIsNotSolvedWhenPuzzleIsIncomplete()
        {
            Assert.IsFalse(_sudoku.IsSolved());
        }

        [Test]
        public void TestSelectCellSelectsCell()
        {
            _sudoku.SelectCell(1, 1);
            Assert.IsTrue(_sudoku.IsSelected(1, 1));
        }

        [Test]
        public void TestDeselectCellDeselectsCell()
        {
            _sudoku.SelectCell(0, 0);
            _sudoku.DeselectCell();
            Assert.IsFalse(_sudoku.IsSelected());
        }

        [Test]
        public void TestResetDeselectsCell()
        {
            _sudoku.SelectCell(1, 1);
            _sudoku.Reset();
            Assert.IsFalse(_sudoku.IsSelected());
        }

        [Test]
        public void TestResetClearsGrid()
        {
            _sudoku.SetValue(1, 1, 1);
            _sudoku.Reset();
            Assert.AreEqual(0, _sudoku.GetCell(1, 1).Value);
        }

        [Test]
        public void TestInvalidCellIsNotDefaultValue()
        {
            Assert.IsFalse(_sudoku.IsDefaultCell(1, 1));
        }

        [Test]
        public void TestUserSetCellIsNotDefaultValue()
        {
            _sudoku.SetValue(1, 1, 1);
            Assert.IsFalse(_sudoku.IsDefaultCell(1, 1));
        }

        [Test]
        public void TestValidNonUserSetCellIsDefaultValue()
        {
            string puzzle = new string('2', 81);
            PuzzleData puzzleData = new PuzzleData(puzzle, _solution);
            _sudoku = new Sudoku(puzzleData, new Point2D());
            Assert.IsTrue(_sudoku.IsDefaultCell(1, 1));
        }

        [Test]
        public void TestSolve()
        {
            _sudoku.Solve();
            Assert.IsTrue(_sudoku.IsSolved());
        }
    }
}