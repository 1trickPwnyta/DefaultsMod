using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Defaults.Schedule
{
    public class DefaultSettingsCategoryWorker_Schedule : DefaultSettingsCategoryWorker
    {
        private List<Schedule> defaultSchedules;
        private int nextScheduleIndex = Mathf.Abs(Rand.Int);

        public DefaultSettingsCategoryWorker_Schedule(DefaultSettingsCategoryDef def) : base(def)
        {
        }

        public Schedule GetNextDefaultSchedule()
        {
            if (defaultSchedules.Any(s => s.use))
            {
                if (nextScheduleIndex >= defaultSchedules.Count)
                {
                    nextScheduleIndex %= defaultSchedules.Count;
                }
                while (!defaultSchedules[nextScheduleIndex].use)
                {
                    nextScheduleIndex++;
                    if (nextScheduleIndex >= defaultSchedules.Count)
                    {
                        nextScheduleIndex %= defaultSchedules.Count;
                    }
                }
                Schedule nextSchedule = defaultSchedules[nextScheduleIndex++];
                DefaultsMod.Settings.Write();
                return nextSchedule;
            }
            else
            {
                return null;
            }
        }

        public override void OpenSettings()
        {
            Find.WindowStack.Add(new Dialog_ScheduleSettings(def));
        }

        protected override bool GetCategorySetting(string key, out object value)
        {
            switch (key)
            {
                case Settings.SCHEDULES:
                    value = defaultSchedules;
                    return true;
                default:
                    value = default;
                    return false;
            }
        }

        protected override bool SetCategorySetting(string key, object value)
        {
            switch (key)
            {
                case Settings.SCHEDULES:
                    defaultSchedules = value as List<Schedule>;
                    return true;
                default:
                    return false;
            }
        }

        public override void ResetSettings()
        {
            base.ResetSettings();
            defaultSchedules = new List<Schedule>()
            {
                new Schedule("Defaults_ScheduleName".Translate(1))
            };
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref defaultSchedules, Settings.SCHEDULES);
            Scribe_Values.Look(ref nextScheduleIndex, Settings.NEXT_SCHEDULE, Mathf.Abs(Rand.Int));
        }
    }
}
