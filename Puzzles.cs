using System;
using System.Collections.Generic;
using System.IO;

namespace sudoku_swe20001
{
    public static class Puzzles
    {
        private static List<PuzzleData> _puzzles = new List<PuzzleData>();

        /// <summary>
        /// Loads a list of puzzles from a csv file
        /// </summary>
        /// <param name="file">File path to csv file</param>
        public static void LoadPuzzles(string file)
        {
            string[] lines = File.ReadAllLines(file);
            foreach (string line in lines)
            {
                string[] data = line.Split(',');
                PuzzleData puzzle = new PuzzleData(data[0], data[1]);
                _puzzles.Add(puzzle);
            }
        }

        /// <summary>
        /// Removes all the puzzles
        /// </summary>
        public static void ClearPuzzles()
        {
            _puzzles.Clear();
        }

        /// <summary>
        /// Gets a random puzzle
        /// </summary>
        public static PuzzleData GetRandomPuzzle()
        {
            if (_puzzles.Count > 0)
            {
                Random r = new Random();
                int index = r.Next(_puzzles.Count);
                return _puzzles[index];
            }
            return null;
        }
    }
}