using System.Collections.Immutable;

namespace BowlingApp.Calculators
{
    public interface IScoreCalculator
    {
        (int score, Note note) CalculateScore(int[] frame, int[][] nextFrames);
        ImmutableList<(int score, Note note)> CalculateScores(int[][] frames);
        ImmutableList<(int score, Note note)> CalculateAccumulatedScores(params int[][] frames);
    }
}