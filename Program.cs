using SplashKitSDK;

namespace sudoku_swe20001
{
    public class Program
    {
        public static void Main()
        {
            SplashKit.LoadFont("Roboto Regular", "fonts/Roboto-Regular.ttf");
            SplashKit.LoadFont("Roboto Bold", "fonts/Roboto-Bold.ttf");
            Puzzles.LoadPuzzles("puzzles.txt");

            Window window = new Window("Sudoku", 800, 600);
            Sudoku sudoku = new Sudoku(Puzzles.GetRandomPuzzle(), SplashKit.ScreenCenter());

            do
            {
                SplashKit.ProcessEvents();
                SplashKit.ClearScreen();

                sudoku.Update();
                sudoku.Draw(window);

                SplashKit.RefreshScreen();
            } while (!SplashKit.WindowCloseRequested("Sudoku"));
        }
    }
}