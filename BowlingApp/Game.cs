using System;
using System.Collections.Generic;
using System.Linq;
using BowlingApp.Calculators;
using BowlingApp.Events;

namespace BowlingApp
{
    public class Game
    {
        public event EventHandler<NewFrameEvent> OnNextFrame;
        public event EventHandler<NewRollEvent> OnNextRoll;
        public event EventHandler<EndEvent> OnEnd;

        private List<int[]> Frames { get; }
        private IScoreCalculator ScoreCalculator { get; } 
        private IGameStateCalculator GameStateCalculator { get; }

        public Game(IScoreCalculator scoreCalculator, IGameStateCalculator gameStateCalculator)
        {
            ScoreCalculator = scoreCalculator;
            GameStateCalculator = gameStateCalculator;
            Frames = new List<int[]>();
        }

        public Game Register(int pinsKnockedDown)
        {
            var newHead = AddOrUpdateHead(pinsKnockedDown, Frames);

            var isLastRollOfFrame = IsLastRollOfFrame(newHead);
            if (isLastRollOfFrame)
            {
                var score = ScoreCalculator.CalculateScore(newHead, new int[0][]);
                NotifyNextFrame(pinsKnockedDown, score);
            }
            else
            {
                NotifyOnNextRoll(pinsKnockedDown);
            }

            if (IsDone())
            {
                NotifyEnd();
            }
            else if (isLastRollOfFrame)
            {
                StartNewFrame(); 
            }

            return this;
        }

        private void NotifyEnd() => 
            OnEnd?.Invoke(this, new EndEvent(GetTotalScore()));

        private void NotifyOnNextRoll(int pinsKnockedDown) =>
            OnNextRoll?.Invoke(
                this, 
                new NewRollEvent(frameNumber: Math.Min(Frames.Count, 10), roll: pinsKnockedDown));

        private void NotifyNextFrame(int pinsKnockedDown, (int score, Note note) score) => 
            OnNextFrame?.Invoke(
                this, 
                new NewFrameEvent(frameNumber: Math.Min(Frames.Count, 10), roll: pinsKnockedDown, score: score.score, bonus: score.note, totalScore: GetTotalScore()));

        private void StartNewFrame() => 
            Frames.Add(new int[0]);

        private bool IsLastRollOfFrame(int[] updatedEntry) => 
            GameStateCalculator.IsLastRollOfFrame(updatedEntry);

        public bool IsDone() => 
            GameStateCalculator.HasGameEnded(Frames.ToArray());

        public int GetTotalScore() => 
            ScoreCalculator.CalculateAccumulatedScores(Frames.ToArray()).Select(x => x.score).LastOrDefault();

        private static int[] AddOrUpdateHead(int pinsKnockedDown, List<int[]> frames)
        {
            var head = frames.LastOrDefault();
            frames.Remove(head);

            var updatedHead = CreateEntry(head ?? new int[0], pinsKnockedDown);
            frames.Add(updatedHead);
            return updatedHead;
        }

        private static int[] CreateEntry(IEnumerable<int> entry, int pinsKnockedDown)
        {
            // TODO: test asserts
            // NOTE: we should consider be encapsulated in a class to enforce these invariants in the constructor
            if (pinsKnockedDown < 0 || pinsKnockedDown > 10)
                throw new ArgumentException($"pins knocked down should be between 0 and 10, it was {pinsKnockedDown}");

            var newEntry = entry.Concat(new[] {pinsKnockedDown}).ToArray();
            if (newEntry.Sum() > 10) 
                throw new ArgumentException($"pins knocked down should be between 0 and 10 in same frame, it was {newEntry.Sum()}");

            return newEntry;
        }
    }
}
