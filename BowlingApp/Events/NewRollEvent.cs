using System;

namespace BowlingApp.Events
{
    public class NewRollEvent : EventArgs
    {
        public NewRollEvent(int frameNumber, int roll)
        {
            FrameNumber = frameNumber;
            Roll = roll;
        }

        public int FrameNumber { get; }
        public int Roll { get; }

        public override bool Equals(object obj)
        {
            return obj is NewRollEvent @event &&
                   FrameNumber == @event.FrameNumber &&
                   Roll == @event.Roll;
        }

        public override int GetHashCode()
        {
            var hashCode = -1176320759;
            hashCode = hashCode * -1521134295 + FrameNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + Roll.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"({FrameNumber}, {Roll})";
        }
    }
}