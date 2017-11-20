using System.Collections.Generic;
using System.Linq;
using BowlingApp.Calculators;
using Shouldly;
using Xunit;

namespace Tests
{
    public class RecursiveScoreCalculatorTests
    {
        // Note: is's usually a good convention to have one assert per test
        // but in this case one could argue the asserts are simple enough to group 
        // and doing so could increase readability
        private readonly RecursiveScoreCalculator calculator;

        public RecursiveScoreCalculatorTests()
        {
            calculator = new RecursiveScoreCalculator();
        }

        [Fact]
        public void NoRollsScoresZero()
        {
            calculator.CalculateScore(new int[] {}, new int[0][])
                .ShouldBe((0, Note.None));
        }

        [Fact]
        public void ScoreEqualsSumOfCurrentFrame()
        {
            calculator.CalculateScore(new []{ 1, 2 }, new int[0][])
                .ShouldBe((3, Note.None));

            calculator.CalculateScore(new[] { 1, 8 }, new[] { new[] { 10, 10 } /* noise */})
                .ShouldBe((9, Note.None));
        }

        [Fact]
        public void SpareIncludeTheNextRollIfAny()
        {
            calculator.CalculateScore(new[] { 9, 1 }, new int[0][])
                .ShouldBe((10, Note.Spare));

            calculator.CalculateScore(new[] { 9, 1 }, new[] { new[] { 2 } })
                .ShouldBe((12, Note.Spare));

            calculator.CalculateScore(new[] { 9, 1 }, new[] { new[] { 2, 9 /* noise */ } })
                .ShouldBe((12, Note.Spare));
        }

        [Fact]
        public void StrikeIncludeTheNextTwoRollsIfAny()
        {
            calculator.CalculateScore(new[] { 10 }, new int[0][])
                .ShouldBe((10, Note.Strike));

            calculator.CalculateScore(new[] { 10 }, new[] { new[] { 2 } })
                .ShouldBe((12, Note.Strike));

            calculator.CalculateScore(new[] { 10 }, new[] { new[] { 2, 9, 3 /* noise */ } })
                .ShouldBe((21, Note.Strike));
        }

        [Fact]
        public void CalculateScoresPerFrame()
        {
            var result = calculator.CalculateScores(
                new [] {1, 4},
                new [] {4, 5},
                new [] {6, 4},
                new [] {5, 5},
                new [] {10},
                new [] {0, 1},
                new [] {7, 3},
                new [] {6, 4},
                new [] {10},
                new [] {2, 8},
                new []{ 6 }).ToList();

            result.ShouldBe(
                    new List<(int score, Note note)>
                    {
                        (5, Note.None),
                        (9, Note.None),
                        (15, Note.Spare),
                        (20, Note.Spare),
                        (11, Note.Strike),
                        (1, Note.None),
                        (16, Note.Spare),
                        (20, Note.Spare),
                        (20, Note.Strike),
                        (16, Note.Spare)
                    }
                );
        }

        [Fact]
        public void CalculateAccumulatedScoresPerFrame()
        {
            var result = calculator.CalculateAccumulatedScores(
                new[] { 1, 4 },
                new[] { 4, 5 },
                new[] { 6, 4 },
                new[] { 5, 5 },
                new[] { 10 },
                new[] { 0, 1 },
                new[] { 7, 3 },
                new[] { 6, 4 },
                new[] { 10 },
                new[] { 2, 8 },
                new[] { 6 }).ToList();

            result.ShouldBe(
                new List<(int score, Note note)>
                {
                    (5, Note.None),
                    (14, Note.None),
                    (29, Note.Spare),
                    (49, Note.Spare),
                    (60, Note.Strike),
                    (61, Note.None),
                    (77, Note.Spare),
                    (97, Note.Spare),
                    (117, Note.Strike),
                    (133, Note.Spare)
                }
            );
        }
    }
}
