using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Defaults.Schedule
{
    public enum TimeAssignment
    {
        Anything,
        Sleep,
        Work,
        Meditate,
        Joy
    }

    public class Schedule : IExposable
    {
        private List<TimeAssignment> assignments;

        public Schedule()
        {
            SetToDefaultSchedule();
        }

        private void SetToDefaultSchedule()
        {
            assignments = new[]
            {
                TimeAssignment.Sleep,
                TimeAssignment.Sleep,
                TimeAssignment.Sleep,
                TimeAssignment.Sleep,
                TimeAssignment.Sleep,
                TimeAssignment.Sleep,
                TimeAssignment.Anything,
                TimeAssignment.Anything,
                TimeAssignment.Anything,
                TimeAssignment.Anything,
                TimeAssignment.Anything,
                TimeAssignment.Anything,
                TimeAssignment.Anything,
                TimeAssignment.Anything,
                TimeAssignment.Anything,
                TimeAssignment.Anything,
                TimeAssignment.Anything,
                TimeAssignment.Anything,
                TimeAssignment.Anything,
                TimeAssignment.Anything,
                TimeAssignment.Anything,
                TimeAssignment.Anything,
                TimeAssignment.Sleep,
                TimeAssignment.Sleep
            }.ToList();
        }

        public void ExposeData()
        {
            Scribe_Collections.Look(ref assignments, "Assignments");
            if (assignments == null)
            {
                SetToDefaultSchedule();
            }
        }

        public TimeAssignment GetTimeAssignment(int hour)
        {
            return assignments[hour];
        }

        public void SetTimeAssignment(int hour, TimeAssignment assignment)
        {
            assignments[hour] = assignment;
        }
    }
}
