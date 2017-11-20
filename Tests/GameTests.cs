using BowlingApp;
using Shouldly;
using System;
using System.Linq;
using BowlingApp.Calculators;
using Xunit;

namespace Tests
{
    public class GameTests
    {
        private readonly Game _game;

        public GameTests()
        {
            _game = new Game(new RecursiveScoreCalculator(), new GameStateCalculator());
        }

        [Fact]
        public void ScoresNoRollsCorrectly()
        {
            _game.GetTotalScore()
                .ShouldBe(0);
        }

        [Fact]
        public void ScoresOneRollsCorrectly()
        {
            _game.Register(1)
                .Register(2);

            _game.GetTotalScore()
                .ShouldBe(3);
        }

        [Fact]
        public void CalculateScoreOfOneFrames()
        {
            _game.Register(1)
                .Register(4);

            _game.GetTotalScore().ShouldBe(1 + 4);
        }

        [Fact]
        public void IsolatedSpareCalculation()
        {
            var g = _game
                .Register(1).Register(4)
                .Register(4).Register(5)
                .Register(6).Register(4)
                .Register(2).Register(5);

            g.GetTotalScore().ShouldBe(
                (1 + 4)
                + (4 + 5)
                + (6 + 4 + 2)
                + (2 + 5));
        }

        [Fact]
        public void IsolatedStrikeCalculation()
        {
            _game
                .Register(1).Register(4)
                .Register(4).Register(5)
                .Register(10)
                .Register(2).Register(5);

            _game.GetTotalScore().ShouldBe(
                (1 + 4)
                + (4 + 5)
                + (10 + 2 + 5)
                + (2 + 5));
        }

        [Fact]
        public void CalculateFinalScore()
        {
            _game
                .Register(1).Register(4)
                .Register(4).Register(5)
                .Register(6).Register(4)
                .Register(5).Register(5)
                .Register(10)
                .Register(0).Register(1)
                .Register(7).Register(3)
                .Register(6).Register(4)
                .Register(10)
                .Register(2).Register(8).Register(6);

            _game.GetTotalScore().ShouldBe(133);
        }

        [Fact]
        public void TenthFrameBonusRoll()
        {
            foreach (var action in Enumerable.Repeat<Action>(() => _game.Register(0), 2 * 9))
                action();

            _game.Register(10).Register(10).Register(10);

            _game.GetTotalScore().ShouldBe(30);
        }

        [Fact]
        public void TenthFrameBonusRollVariation()
        {
            foreach (var action in Enumerable.Repeat<Action>(() => _game.Register(0), 2 * 9))
                action();

            _game.Register(1).Register(9).Register(10);

            _game.GetTotalScore().ShouldBe(20);
        }
    }
}
