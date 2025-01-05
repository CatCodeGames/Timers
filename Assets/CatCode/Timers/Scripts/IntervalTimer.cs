using System;


namespace CatCode.Timers
{
    public sealed partial class IntervalTimer
    {
        private readonly UpdateMode _updateMode;
        private readonly bool _unscaledTime;

        private bool _inSystem = false;
        private bool _isActive = false;

        public float ElapsedTime = 0f;
        public float Interval = 0f;

        public int TotalLoops = -1;
        public int CompletedLoops = 0;

        public bool MultiInvokeOnUpdate = false;
        public Action Callback = null;

        public float ElapsedRatio
            => ElapsedTime / Interval;

        public bool IsActive => _isActive;

        public IntervalTimer(float interval, Action callback, UpdateMode updateMode = UpdateMode.RegularUpdate, bool unscaledTime = false, bool multiInvokeOnUpdate = false)
        {
            Interval = interval;
            Callback = callback;
            MultiInvokeOnUpdate = multiInvokeOnUpdate;

            _updateMode = updateMode;
            _unscaledTime = unscaledTime;
        }

        public void Start()
        {
            if (_isActive || Callback == null)
                return;
            _isActive = true;
            if (!_inSystem)
                TimerSystem.RegisterTimer(this, _updateMode, _unscaledTime);
        }

        public void Stop()
        {
            if (!_isActive)
                return;
            _isActive = false;
            if (_inSystem)
                TimerSystem.ScheduleCleaningSystem(_updateMode, _unscaledTime);
        }

        public void Reset()
        {
            ElapsedTime = 0f;
            CompletedLoops = 0;


        }

        public IntervalTimer SetInterval(float interval)
        {
            Interval = interval;
            return this;
        }

        public IntervalTimer SetCallback(Action callback)
        {
            Callback = callback;
            return this;
        }

        public IntervalTimer SetLoops(int loops)
        {
            TotalLoops = loops;
            return this;
        }
    }
}