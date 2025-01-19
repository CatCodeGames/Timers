using UnityEngine;

namespace CatCode.Timers
{
    public class TimerTester : MonoBehaviour
    {
        private IntervalTimer _timer;
        [SerializeField] private float _interval = 1;
        [SerializeField] private int _loops = -1;
        [SerializeField] private bool _reset;

        private void Awake()
        {
            _timer = new IntervalTimer(_interval, OnElapsed, _loops, UpdateMode.RegularUpdate, TimeMode.Scaled, true);
        }

        private void OnEnable()
        {
            var timer = IntervalTimerPool.Get(_interval, OnElapsed, _loops, UpdateMode.RegularUpdate, TimeMode.Scaled, false);
            _timer.Reset();
            _timer.Start();
            _timer.Stop();
            _timer.Start();
            _timer.Stop();
            _timer.Start();
        }

        private void OnDisable()
        {
            IntervalTimerPool.Release(_timer);
        }

        private void OnElapsed()
        {
            if (_reset)
                _timer.CompletedLoops = 0;
            Debug.Log($"{Time.time}");
        }
    }
}
