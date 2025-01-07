using UnityEngine;
using UnityEngine.PlayerLoop;


namespace CatCode.Timers
{
    public static class TimerSystem
    {
        private static IntervalTimer.IntervalTimerSystem[,] _systems;

        public const int UpdateModeCount = 3;
        public const int TimeModeCount = 2;

        public const int RegularUpdateIndex = 0;
        public const int FixedUpdateIndex = 1;
        public const int LateUpdateIndex = 2;

        public const int UnscaledTimeIndex = 0;
        public const int ScaledTimeIndex = 1;

        static TimerSystem()
        {
            _systems = new IntervalTimer.IntervalTimerSystem[UpdateModeCount, TimeModeCount];
            for (int i = 0; i < UpdateModeCount; i++)
                for (int j = 0; j < TimeModeCount; j++)
                    _systems[i, j] = new IntervalTimer.IntervalTimerSystem();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Initializer()
        {
            PlayerLoopUtils.AddLoopSystem<IntervalTimer.IntervalTimerSystem>(new[] { typeof(Update) }, Update);
            PlayerLoopUtils.AddLoopSystem<IntervalTimer.IntervalTimerSystem>(new[] { typeof(FixedUpdate) }, FixedUpdate);
            PlayerLoopUtils.AddLoopSystem<IntervalTimer.IntervalTimerSystem>(new[] { typeof(PreLateUpdate) }, LateUpdate);
        }


        public static void RegisterTimer(IntervalTimer timer, UpdateMode updateMode = UpdateMode.RegularUpdate, bool unscaled = false)
        {
            var system = Get(updateMode, unscaled);
            system.Add(timer);
        }

        public static void ScheduleCleaningSystem(UpdateMode updateMode = UpdateMode.RegularUpdate, bool unscaled = false)
        {
            var system = Get(updateMode, unscaled);
            system.ScheduleInactiveTimersRemoval();
        }

        private static void FixedUpdate()
        {
            _systems[FixedUpdateIndex, ScaledTimeIndex].Update(Time.fixedDeltaTime);
            _systems[FixedUpdateIndex, UnscaledTimeIndex].Update(Time.fixedUnscaledDeltaTime);
        }

        private static void Update()
        {
            _systems[RegularUpdateIndex, ScaledTimeIndex].Update(Time.deltaTime);
            _systems[RegularUpdateIndex, UnscaledTimeIndex].Update(Time.unscaledDeltaTime);
        }

        private static void LateUpdate()
        {
            _systems[LateUpdateIndex, ScaledTimeIndex].Update(Time.deltaTime);
            _systems[LateUpdateIndex, UnscaledTimeIndex].Update(Time.unscaledDeltaTime);
        }

        private static IntervalTimer.IntervalTimerSystem Get(UpdateMode mode, bool unscaled)
        {
            var updateModeIndex = RegularUpdateIndex;
            switch (mode)
            {
                case UpdateMode.RegularUpdate: updateModeIndex = RegularUpdateIndex; break;
                case UpdateMode.LateUpdate: updateModeIndex = LateUpdateIndex; break;
                case UpdateMode.FixedUpdate: updateModeIndex = FixedUpdateIndex; break;
            }
            var timeScaleIndex = unscaled ? UnscaledTimeIndex : ScaledTimeIndex;
            return _systems[updateModeIndex, timeScaleIndex];
        }
    }
}