using System;
using System.Collections.Generic;
using System.Linq;
using BowlingApp.Calculators;
using Shouldly;
using Xunit;

namespace Tests.Calculators
{
    public class GameStateCalculatorTests
    {
        // Note: is's usually a good convention to have one assert per test
        // but in this case one could argue the asserts are simple enough to group 
        // and doing so could increase readability
        private readonly GameStateCalculator _calculator;

        public GameStateCalculatorTests()
        {
            _calculator = new GameStateCalculator();
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, false)]
        [InlineData(2, false)]
        [InlineData(3, false)]
        [InlineData(4, false)]
        [InlineData(5, false)]
        [InlineData(6, false)]
        [InlineData(7, false)]
        [InlineData(8, false)]
        [InlineData(9, false)]
        [InlineData(10, true)]
        public void HasStandardGameEnded(int frameCount, bool expected)
        {
            _calculator.HasGameEnded(
                Repeat(RandomNormalFrame, frameCount).ToArray()).ShouldBe(expected);
        }

        [Fact]
        public void Bug_HasStandardGameEndedInTenthFrameFirstNormalRoll()
        {
            var frames = Repeat(RandomNormalFrame, 9).ToList();
            frames.Add(new[] {1});

            _calculator.HasGameEnded(frames.ToArray())
                .ShouldBe(false);
        }

        [Fact]
        public void Bug_HasStandardGameEndedInTenthFrameWithSpare()
        {
            var frames = Repeat(RandomNormalFrame, 9).ToList();
            frames.Add(new[] { 1, 9 });
            frames.Add(new int[0]);

            _calculator.HasGameEnded(frames.ToArray())
                .ShouldBe(false);
        }

        [Fact]
        public void HasGameWithSpareInFinalFrameEnded()
        {
            var frames = Repeat(RandomNormalFrame, 9).ToList();
            frames.Add(RandomSpareFrame());

            _calculator.HasGameEnded(frames.ToArray()).ShouldBe(false);

            frames.Add(new[] { 9 });
            _calculator.HasGameEnded(frames.ToArray()).ShouldBe(true);
        }

        [Fact]
        public void HasGameWithStrikeInFinalFrameEnded()
        {
            var frames = Repeat(RandomNormalFrame, 9).ToList();
            frames.Add(new[] { 10 });

            _calculator.HasGameEnded(frames.ToArray()).ShouldBe(false);

            frames.Add(new[] { 9 });
            _calculator.HasGameEnded(frames.ToArray()).ShouldBe(false);

            frames.Add(new[] { 9 });
            _calculator.HasGameEnded(frames.ToArray()).ShouldBe(true);
        }

        [Fact]
        public void Bug_HasGameWithStrikeInFinalFrameEndedAndBonusRollsWhereStrikes()
        {
            var frames = Repeat(RandomNormalFrame, 9).ToList();
            frames.Add(new[] { 10 });

            _calculator.HasGameEnded(frames.ToArray()).ShouldBe(false);

            frames.Add(new[] { 10 });
            _calculator.HasGameEnded(frames.ToArray()).ShouldBe(false);

            frames.Add(new[] { 10 });
            _calculator.HasGameEnded(frames.ToArray()).ShouldBe(true);
        }

        [Fact]
        public void IsLastRollOfFrame()
        {
            _calculator.IsLastRollOfFrame(new [] { 10 }).ShouldBe(true);
            _calculator.IsLastRollOfFrame(new [] { 1, 9}).ShouldBe(true);
            _calculator.IsLastRollOfFrame(new [] { 3 }).ShouldBe(false);
        }

        private static IEnumerable<T> Repeat<T>(Func<T> creator, int count)
        {
            return Enumerable.Repeat(creator, count).Select(f => f());
        }

        private static int[] RandomNormalFrame()
        {
            var random = new Random();
            var firstRoll = random.Next(0, 9);
            var secondRoll = random.Next(0, 9 - firstRoll);
            return new[] {firstRoll, secondRoll};
        }

        private static int[] RandomSpareFrame()
        {
            var random = new Random();
            var firstRoll = random.Next(0, 9);
            var secondRoll = 10 - firstRoll;
            return new[] { firstRoll, secondRoll };
        }
    }
}
