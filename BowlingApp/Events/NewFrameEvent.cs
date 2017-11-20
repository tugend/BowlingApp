using System;
using BowlingApp.Calculators;

namespace BowlingApp.Events
{
    public class NewFrameEvent : EventArgs
    {
        public NewFrameEvent(int frameNumber, int roll, int score, Note bonus, int totalScore)
        {
            FrameNumber = frameNumber;
            Roll = roll;
            Score = score;
            Bonus = bonus;
            TotalScore = totalScore;
        }

        public int FrameNumber { get; }
        public int Roll { get; }
        public int Score { get; }
        public Note Bonus { get; }
        public int TotalScore { get; }

        public override bool Equals(object obj)
        {
            return obj is NewFrameEvent @event &&
                   FrameNumber == @event.FrameNumber &&
                   Roll == @event.Roll &&
                   Score == @event.Score &&
                   Bonus == @event.Bonus &&
                   TotalScore == @event.TotalScore;
        }

        public override int GetHashCode()
        {
            var hashCode = -809057385;
            hashCode = hashCode * -1521134295 + FrameNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + Roll.GetHashCode();
            hashCode = hashCode * -1521134295 + Score.GetHashCode();
            hashCode = hashCode * -1521134295 + Bonus.GetHashCode();
            hashCode = hashCode * -1521134295 + TotalScore.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"({FrameNumber}, {Roll}, {Score}, {Bonus}, {TotalScore})";
        }
    }
}