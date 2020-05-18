using NUnit.Framework;

namespace sudoku_swe20001
{
    [TestFixture]
    public class TestGrid
    {
        private Grid _grid;

        [SetUp]
        public void SetUp()
        {
            _grid = new Grid();
        }

        [TestCase(0, 0)]
        [TestCase(8, 8)]
        public void TestGetValidCell(int x, int y)
        {
            Cell cell = _grid.GetCell(x, y);
            Assert.IsNotNull(cell);
        }

        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        [TestCase(9, 8)]
        [TestCase(8, 9)]
        public void TestGetInvalidCell(int x, int y)
        {
            Cell cell = _grid.GetCell(x, y);
            Assert.IsNull(cell);
        }

        [TestCase(1, 1, 1)]
        public void TestSetValue(int x, int y, int value)
        {
            _grid.SetValue(x, y, value);
            int newValue = _grid.GetCell(x, y).Value;
            Assert.AreEqual(value, newValue);
        }

        [Test]
        public void TestReset()
        {
            _grid.SetValue(1, 1, 1);
            _grid.Reset();
            int value = _grid.GetCell(1, 1).Value;
            Assert.AreNotEqual(1, value);
        }
    }
}