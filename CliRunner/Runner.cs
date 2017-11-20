using System;
using System.Collections.Generic;
using System.Linq;
using BowlingApp;
using BowlingApp.Calculators;
using BowlingApp.Events;

namespace CliRunner
{
    internal class Runner
    {
        private readonly Game _game;

        public Runner()
        {
            _game = new Game(new RecursiveScoreCalculator(), new GameStateCalculator());
        }

        public void Start(string[] args)
        {
            Console.WriteLine("{0,-10}", "START");
            Console.WriteLine("{0,-10}{1,10}{2,10}{3,20}{4,40}", "FRAME", "PINS", "SCORE", "TOTAL SCORE", "BONUS");
            Console.WriteLine("".PadLeft(90, '-'));

            _game.OnNextRoll += OnNextRollHandler();
            _game.OnNextFrame += OnNextFrameHandler();
            _game.OnEnd += OnEndHandler();

            // TODO: Make modular to allow for different promts perhaps?
            // TODO: use a state pattern here perhaps?
            foreach (var value in ParseInitialInput(args)) Register(value);
            while (Promt()) { }
        }
        private void Register(int value)
        {
            if (value < 0 || value > 10)
            {
                Console.WriteLine("Invalid value");
                return;
            }

            _game.Register(value);
            Console.WriteLine();
        }

        private bool Promt()
        {
            if (_game.IsDone())
            {
                EndGamePromt();
                return false;
            }

            GamePromt();
            return true;
        }

        private void GamePromt()
        {
            Console.WriteLine();
            Console.Write("input next roll> ");

            if (int.TryParse(Console.ReadLine(), out var value))
            {
                ClearLastLines(2);
                Register(value);
            }
            else
            {
                Console.Write("Could not parse value.");
            }
        }

        private static void EndGamePromt()
        {
            Console.WriteLine("Press Q to end game");

            while (Console.ReadLine().ToLower() != "q") { }
        }

        private static IEnumerable<int> ParseInitialInput(IEnumerable<string> args)
        {
            return args.Select(x =>
            {
                if (int.TryParse(x, out var value))
                    return value;

                throw new ArgumentException($"Could not parse {x} as a number");
            });
        }

        private static void ClearLastLines(int count)
        {
            var currentLineCursor = Console.CursorTop;

            for (var i = 0; i < count; i++)
            {
                Console.SetCursorPosition(0, Console.CursorTop - i - 1);
                Console.Write(new string(' ', Console.WindowWidth));
            }

            Console.SetCursorPosition(0, currentLineCursor - count);
        }

        private EventHandler<EndEvent> OnEndHandler() => (sender, e) =>
        {
            Console.WriteLine("{0,30}{1,40}", _game.GetTotalScore(), "final score");
            Console.WriteLine();
        };

        private static EventHandler<NewFrameEvent> OnNextFrameHandler() => (sender, e) =>
        {
            var note = "";

            switch (e.Bonus)
            {
                case Note.Strike:
                    note = "+ bonus from strike";
                    break;
                case Note.Spare:
                    note = "+ bonus from spare";
                    break;
            }

            Console.Write("{0,-10}{1,10}{2,10}{3,20}{4,40}", e.FrameNumber, e.Roll, e.Score, e.TotalScore, e.Bonus == Note.None
                ? ""
                : note);
        };

        private static EventHandler<NewRollEvent> OnNextRollHandler() => (sender, e) =>
        {
            Console.Write("{0,-10}{1,10}", e.FrameNumber, e.Roll);
        };
    }
}
