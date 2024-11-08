

using UnityEngine;

namespace CatCode.Timers
{
    public class NewTimerTester : MonoBehaviour
    {
        private IntervalTimer _timer;
        [SerializeField] private float _interval;

        private void Awake()
        {
            _timer = new IntervalTimer(_interval, OnElapsed)
                .SetLoops(1);
        }

        private void OnEnable()
        {
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
            Debug.Log(Time.time);
        }
    }
}
