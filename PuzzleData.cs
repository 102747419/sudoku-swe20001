namespace sudoku_swe20001
{
    public class PuzzleData
    {
        public readonly string Puzzle;
        public readonly string Solution;

        public PuzzleData(string puzzle, string solution)
        {
            Puzzle = puzzle;
            Solution = solution;
        }
    }
}