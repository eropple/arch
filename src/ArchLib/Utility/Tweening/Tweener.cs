using System;
using System.Collections.Generic;

namespace ArchLib.Utility.Tweening
{
    public class Tweener
    {
        public readonly Boolean Repeating;
        public readonly Boolean Mirrored;
        public readonly Double Duration;
        public readonly Double Start;
        public readonly Double End;
        public readonly TweenFunction Function;

        private Boolean _finished;
        private Double _time;
        private Double _currentValue;

        public Tweener(Double start, Double end, Double duration, TweenFunction function)
            : this(start, end, duration, function, false, false)
        {
            _currentValue = start;
        }


        public Tweener(Double start, Double end, Double duration, TweenFunction function, Boolean mirrored, Boolean repeating)
        {
            Start = start;
            End = end;

            Duration = duration;
            Function = function;

            Mirrored = mirrored;
            Repeating = repeating;
        }

        public Boolean Finished { get { return _finished; } }

        public Double Time
        {
            get { return _time; }
        }
        public void Reset(Double resetToTime = 0)
        {
            _time = resetToTime;
        }

        public void Update(Double delta)
        {
            _time += delta;
            _finished = (!Repeating) && (Mirrored ? (_time > 2 * Duration) : (_time > Duration));

            if (_finished)
            {
                _currentValue = Mirrored ? Start : End;
                return;
            }

            if (_time <= 0)
            {
                _currentValue = Start;
                return;
            }


            if (Repeating && !Mirrored)
                _time = _time % Duration;
            else if (Repeating)
                _time = _time % (Duration * 2);

            if (Mirrored && _time > Duration)
                _currentValue = Function(_time - Duration, End, Start, Duration);
            else
                _currentValue = Function(_time, Start, End, Duration);
        }

        public Double Value { get { return _currentValue; } }

        public override string ToString()
        {
            return String.Format("Tweener: time {0}\t\t\t\tvalue {1} (finished: {2})", _time, _currentValue, _finished);
        }


        public static Double Linear(Double time, Double start, Double end, Double duration)
        {
            return ((end - start) * time) / (duration) + start;
        }
    }

    public delegate Double TweenFunction(Double time, Double start, Double end, Double duration);
}
