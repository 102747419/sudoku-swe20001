using NUnit.Framework;

namespace sudoku_swe20001
{
    [TestFixture]
    public class TestCell
    {
        private Cell _cell;

        [SetUp]
        public void SetUp()
        {
            _cell = new Cell(0, 0);
        }

        [TestCase(1)]
        [TestCase(9)]
        public void TestHasValidValue(int value)
        {
            _cell.Value = value;
            Assert.IsTrue(_cell.HasValue());
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(10)]
        public void TestHasInvalidValue(int value)
        {
            _cell.Value = value;
            Assert.IsFalse(_cell.HasValue());
        }
    }
}