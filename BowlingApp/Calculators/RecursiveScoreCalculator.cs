using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace BowlingApp.Calculators
{
    public class RecursiveScoreCalculator : IScoreCalculator
    {
        public (int score, Note note) CalculateScore(int[] frame, int[][] nextFrames)
        {
            if (!frame.Any())
                return (0, Note.None);

            var sum = frame.Sum();

            if (sum != 10) return (sum, Note.None);

            var rolls = nextFrames.SelectMany(x => x);

            // TODO: spare and strike should not be allowed in the final bonus rolls
            return frame.Length == 2
                ? (sum + rolls.Take(1).Sum(), Note.Spare)
                : (sum + rolls.Take(2).Sum(), Note.Strike);
        }

        // ReSharper disable once FunctionRecursiveOnAllPaths
        private IEnumerable<(int score, Note note)> CalculateScoresRec(int[][] frames)
        {
            if (!frames.Any())
                return Enumerable.Empty<(int score, Note note)>();

            var first = new[] { CalculateScore(frames.First(), frames.Skip(1).ToArray()) };
            var rest = CalculateScoresRec(frames.Skip(1).ToArray());

            return first.Concat(rest);
        }

        public ImmutableList<(int score, Note note)> CalculateScores(params int[][] frames)
        {
            return CalculateScoresRec(frames).Take(10).ToImmutableList();
        }

        public ImmutableList<(int score, Note note)> CalculateAccumulatedScores(params int[][] frames)
        {
            var seed = new (int score, Note note)[0];

            (int score, Note note)[] AggregateFunc((int score, Note note)[] accum, (int score, Note note) result) => 
                accum.Concat(new[] { (result.score + accum.LastOrDefault().score, result.note) }).ToArray();

            return CalculateScores(frames).Aggregate(seed, AggregateFunc).ToImmutableList();
        }
    }
}