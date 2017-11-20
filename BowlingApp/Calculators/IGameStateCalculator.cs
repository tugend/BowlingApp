namespace BowlingApp.Calculators
{
    public interface IGameStateCalculator
    {
        bool HasGameEnded(int[][] frames);
        bool IsLastRollOfFrame(int[] frame);
    }
}