using UnityEngine;


namespace CatCode.Timers
{
    public sealed partial class IntervalTimer
    {
        public sealed class TimerManager : MonoBehaviour
        {
            private static bool _isShuttingDown = false;

            public const int UpdateModeCount = 3;
            public const int TimeModeCount = 2;

            public const int UpdateIndex = 0;
            public const int FixedUpdateIndex = 1;
            public const int LateUpdateIndex = 2;

            public const int UnscaledTimeIndex = 0;
            public const int ScaledTimeIndex = 1;

            private TimerSystem[,] _systems;


            public void RegisterTimer(IntervalTimer timerData, UpdateMode updateMode = UpdateMode.NormalUpdate, bool unscaled = false)
            {
                var system = Get(updateMode, unscaled);
                system.Add(timerData);
            }

            public void ScheduleCleaningSystem(UpdateMode updateMode = UpdateMode.NormalUpdate, bool unscaled = false)
            {
                var system = Get(updateMode, unscaled);
                system.ScheduleInactiveTimersRemoval();
            }


            private TimerSystem Get(UpdateMode mode, bool unscaled)
            {
                var updateModeIndex = UpdateIndex;
                switch (mode)
                {
                    case UpdateMode.NormalUpdate: updateModeIndex = UpdateIndex; break;
                    case UpdateMode.LateUpdate: updateModeIndex = LateUpdateIndex; break;
                    case UpdateMode.FixedUpdate: updateModeIndex = FixedUpdateIndex; break;
                }
                var timeScaleIndex = unscaled ? UnscaledTimeIndex : ScaledTimeIndex;
                return _systems[updateModeIndex, timeScaleIndex];
            }



            private static TimerManager _instance;

            public static TimerManager Instance
            {
                get
                {
                    if (_isShuttingDown)
                        return null;
                    if (_instance == null)
                    {
                        var go = new GameObject(typeof(TimerManager).Name, typeof(TimerManager));
                        go.hideFlags = HideFlags.HideAndDontSave;
                        _instance = go.GetComponent<TimerManager>();
                    }
                    return _instance;
                }
            }

            public static void Register(IntervalTimer timerData, UpdateMode updateMode, bool unscaled)
                => Instance?.RegisterTimer(timerData, updateMode, unscaled);

            public static void ScheduleCleaning(UpdateMode updateMode, bool unscaled)
                => Instance?.ScheduleCleaningSystem(updateMode, unscaled);


            private void Awake()
            {
                if (_instance == null)
                    _instance = gameObject.GetComponent<TimerManager>();
                else
                {
                    Destroy(gameObject);
                    return;
                }
                _systems = new TimerSystem[UpdateModeCount, TimeModeCount];
                for (int i = 0; i < UpdateModeCount; i++)
                    for (int j = 0; j < TimeModeCount; j++)
                        _systems[i, j] = new TimerSystem();
            }

            private void Update()
            {
                _systems[UpdateIndex, ScaledTimeIndex].Update(Time.deltaTime);
                _systems[UpdateIndex, UnscaledTimeIndex].Update(Time.unscaledDeltaTime);
            }

            private void FixedUpdate()
            {
                _systems[FixedUpdateIndex, ScaledTimeIndex].Update(Time.fixedDeltaTime);
                _systems[FixedUpdateIndex, UnscaledTimeIndex].Update(Time.fixedUnscaledDeltaTime);
            }

            private void LateUpdate()
            {
                _systems[LateUpdateIndex, ScaledTimeIndex].Update(Time.deltaTime);
                _systems[LateUpdateIndex, UnscaledTimeIndex].Update(Time.unscaledDeltaTime);
            }

            private void OnApplicationQuit()
            {
                _isShuttingDown = true;
            }

            private void OnApplicationPause(bool pause)
            {
                _isShuttingDown = pause;
            }
        }
    }
}
