using BowlingApp;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using BowlingApp.Calculators;
using BowlingApp.Events;
using Xunit;

namespace Tests
{
    public class GameEventTests
    {
        private readonly Game _game;
        private readonly Queue<NewFrameEvent> _observedNewFrameEvents = new Queue<NewFrameEvent>();
        private readonly Queue<NewRollEvent> _observedNextRollEvents = new Queue<NewRollEvent>();
        private readonly Queue<EndEvent> _observedEndEvents = new Queue<EndEvent>();

        public GameEventTests()
        {
            _game = new Game(new RecursiveScoreCalculator(), new GameStateCalculator());
            _game.OnNextFrame += (_, e) => _observedNewFrameEvents.Enqueue(e);
            _game.OnNextRoll += (_, e) => _observedNextRollEvents.Enqueue(e);
            _game.OnEnd += (_, e) => _observedEndEvents.Enqueue(e);
        }

        [Fact]
        public void NotifyNextFrame()
        {
            _game.Register(1).Register(8);
            ShouldObserveEvent(new NewFrameEvent(1, 8, 9, Note.None, 9));
        }

        [Fact]
        public void NotifyNextFrameGivenSpare()
        {
            _game.Register(1).Register(9);
            ShouldObserveEvent(new NewFrameEvent(1, 9, 10, Note.Spare, 10));
        }

        [Fact]
        public void NotifyNextFrameGivenStrike()
        {
            _game.Register(10);
            ShouldObserveEvent(new NewFrameEvent(1, 10, 10, Note.Strike, 10));
        }

        [Fact]
        public void NotifyNextFrameScenario()
        {
            _game
                .Register(1).Register(8)
                .Register(10)
                .Register(0).Register(10)
                .Register(1).Register(2);

            ShouldObserveEvents(
                new NewFrameEvent(1, 8, 9, Note.None, 9),
                new NewFrameEvent(2, 10, 10, Note.Strike, 19),
                new NewFrameEvent(3, 10, 10, Note.Spare, 39),
                new NewFrameEvent(4, 2, 3, Note.None, 43)
             );
        }

        [Fact]
        public void NotifyOnNextRoll()
        {
            _game
                .Register(1).Register(8)
                .Register(10)
                .Register(0).Register(10)
                .Register(1).Register(2)
                .Register(3);

            ShouldObserveEvents(
                new NewRollEvent(1, 1),
                new NewRollEvent(3, 0),
                new NewRollEvent(4, 1),
                new NewRollEvent(5, 3)
            );
        }

        [Fact]
        public void NotifyEnd()
        {
            throw new NotImplementedException("TODO");
        }

        [Fact]
        public void TenthFrameOfGameMayHaveThreeRolls()
        {
            throw new NotImplementedException("TODO");
        }

        private void ShouldObserveEvent(NewFrameEvent expected)
        {
            _observedNewFrameEvents.Single().ShouldBe(expected);
            _observedNewFrameEvents.Clear();
        }

        private void ShouldObserveEvents(params NewFrameEvent[] expected)
        {
            _observedNewFrameEvents.ShouldBe(expected);
            _observedNewFrameEvents.Clear();
        }

        private void ShouldObserveEvents(params NewRollEvent[] expected)
        {
            _observedNextRollEvents.ShouldBe(expected);
            _observedNextRollEvents.Clear();
        }
    }
}
