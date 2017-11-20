using System.Linq;

namespace BowlingApp.Calculators
{
    public class GameStateCalculator : IGameStateCalculator
    {
        // TODO: add some asserts wrt. valid number of frames
        public bool HasGameEnded(params int[][] frames) => 
            frames.Length == 10 && frames.Last().Sum() < 10 && frames.Last().Length == 2|| // TENTH FRAME, NO BONUS ROLL 
            frames.Length == 11 && IsSpare(frames.TakeLast(2).First()) && frames.Last().Length == 1 || // ELLEVENTH 'FRAME', SINGLE BONUS ROLL 
            frames.Length == 12;

        private static bool IsSpare(int[] frame)
        {
            return frame.Sum() == 10 && frame.Length == 2;
        }

        // It's upto discussion whether this belongs in a separate calculator
        public bool IsLastRollOfFrame(int[] frame) =>
            frame.Sum() == 10 || frame.Length == 2;
    }
}
