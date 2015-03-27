using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Devoxx.Model;

namespace Devoxx.Data
{
   
    public sealed class ScheduleDataSource
    {
        public event EventHandler SchedulesLoaded;
        public bool IsLoaded = false;

        private static ScheduleDataSource _scheduleDataSource = new ScheduleDataSource();
        private ObservableCollection<Schedule> schedules = new ObservableCollection<Schedule>();
        public ObservableCollection<Schedule> Schedules
        {
            get { return this.schedules; }
        }

        private ObservableCollection<Index> hoursCriteria = new ObservableCollection<Index>();

        public ObservableCollection<Index> HoursCriteria
        {
            get { return this.hoursCriteria; }
        }

        public static async Task<Schedule> GetScheduleAsync(string day)
        {
            await _scheduleDataSource.GetScheduleDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = _scheduleDataSource.Schedules.Where(s => s.Day == day);
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static async Task<Index> GetHoursIndex(string day)
        {
            if (!_scheduleDataSource.Schedules.Any())
                await _scheduleDataSource.GetScheduleDataAsync();
            var matches = _scheduleDataSource.HoursCriteria.Where(s => s.Key == day);
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static async Task<Slot> GetSlotAsync(string uniqueId)
        {
            if (!_scheduleDataSource.Schedules.Any())
                await _scheduleDataSource.GetScheduleDataAsync();
            // Simple linear search is acceptable for small data sets
            List<Slot> slots = _scheduleDataSource.Schedules.SelectMany(schedule => schedule.Slots).Distinct().ToList();
            var matches = slots.Where(slot => slot.Id == uniqueId).ToList();
            return matches.FirstOrDefault();
        }
        

        public async Task GetScheduleDataAsync()
        {
            if (!IsLoaded)
            {
                IEnumerable<string> pathes = new List<string>
                {
                    "http://cfp.devoxx.fr/api/conferences/DevoxxFR2015/schedules/wednesday/",
                    "http://cfp.devoxx.fr/api/conferences/DevoxxFR2015/schedules/thursday/",
                    "http://cfp.devoxx.fr/api/conferences/DevoxxFR2015/schedules/friday/"
                };
                string jsonText;

                using (var client = new HttpClient())
                {
                    foreach (var path in pathes)
                    {
                        var response = await client.GetAsync(path);
                        jsonText = await response.Content.ReadAsStringAsync();
                        var schedule = Utils.DeserializeSchedule(jsonText);
                        schedules.Add(schedule);

                        
                        var item = Utils.CreateIndex(schedule, HourCriteria());
                        hoursCriteria.Add(item);
                    }
                }

                // Fire a SchedulesLoaded event
                if (SchedulesLoaded != null)
                    SchedulesLoaded(this, null);

                IsLoaded = true;
            }
        }

        private Func<Slot, string> HourCriteria()
        {
            return s => s.FromTimeToTime;
        }


        public static IEnumerable<Slot> GetScheduleOfHourAsync(string day, string hour)
        {
            var schedule = _scheduleDataSource.Schedules.FirstOrDefault(s => s.Day == day);
            var slots = schedule.Slots;
            
            return slots.Where(s => s.FromTimeToTime == hour);
        }
    }
}
