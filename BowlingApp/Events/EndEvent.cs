using System;

namespace BowlingApp.Events
{
    public class EndEvent : EventArgs
    {
        public EndEvent(int finalScore)
        {
            FinalScore = finalScore;
        }
        public int FinalScore { get; }
    }
}