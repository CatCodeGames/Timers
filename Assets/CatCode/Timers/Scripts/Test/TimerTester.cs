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
            _timer = new IntervalTimer(_interval, OnElapsed, UpdateMode.RegularUpdate, multiInvokeOnUpdate: true)
                .SetLoops(_loops);
        }

        private void OnEnable()
        {
            _timer.Reset();
            _timer.Start();
            _timer.Stop();
            _timer.Start();
            _timer.Stop();
            _timer.Start();
        }

        private void OnDisable()
        {
            _timer.Stop();
        }

        private void OnElapsed()
        {
            if (_reset)
                _timer.CompletedLoops = 0;
            Debug.Log($"{Time.time} - {_timer.ElapsedTime}");
        }
    }
}
