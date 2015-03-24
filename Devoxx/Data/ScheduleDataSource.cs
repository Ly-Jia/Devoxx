using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
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

        public static async Task<IEnumerable<Schedule>> GetGroupsAsync()
        {
            await _scheduleDataSource.GetScheduleDataAsync();

            return _scheduleDataSource.Schedules;
        }

        public static async Task<Schedule> GetGroupAsync(string day)
        {
            await _scheduleDataSource.GetScheduleDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = _scheduleDataSource.Schedules.Where((schedule) => schedule.Day.Equals(day));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static async Task<Slot> GetItemAsync(string uniqueId)
        {
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
                        await AddSchedule(schedule);
                    }
                }

                // Fire a SchedulesLoaded event
                if (SchedulesLoaded != null)
                    SchedulesLoaded(this, null);

                IsLoaded = true;
            }
        }

        private async Task AddSchedule(Schedule schedule)
        {
            schedules.Add(schedule);
        }
    }
}
