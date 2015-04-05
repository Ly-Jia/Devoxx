using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;
using Devoxx.Model;
using UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding;

namespace Devoxx.Data
{
   
    public sealed class ScheduleDataSource
    {
        private StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        private static Uri wednesdayUri = new Uri("ms-appx:///Data/Wednesday.json");
        private static Uri thursdayUri = new Uri("ms-appx:///Data/Thursday.json");
        private static Uri fridayUri = new Uri("ms-appx:///Data/Friday.json");

        public event EventHandler SchedulesLoaded;
        public bool IsLoaded = false;

        
        private static ScheduleDataSource _scheduleDataSource = new ScheduleDataSource();
        private ObservableCollection<Schedule> schedules = new ObservableCollection<Schedule>();
        public ObservableCollection<Schedule> Schedules
        {
            get { return this.schedules; }
        }

        private ObservableCollection<Index> hoursIndex = new ObservableCollection<Index>();
        public ObservableCollection<Index> HoursIndex
        {
            get { return this.hoursIndex; }
        }

        private ObservableCollection<Index> roomsIndex = new ObservableCollection<Index>();

        public ObservableCollection<Index> RoomsIndex
        {
            get { return this.roomsIndex; }
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
           
            var matches = _scheduleDataSource.HoursIndex.Where(s => s.Key == day);
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static async Task<Index> GetRoomsIndex(string day)
        {
           
            var matches = _scheduleDataSource.RoomsIndex.Where(s => s.Key == day);
            if (matches.Count() == 1) return matches.First();
            return null;
        }
        public static async Task<Slot> GetSlotAsync(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            List<Slot> slots = _scheduleDataSource.Schedules.SelectMany(schedule => schedule.Slots).Distinct().ToList();
            var matches = slots.Where(slot => slot.Id == uniqueId).ToList();
            return matches.FirstOrDefault();
        }
        

        public async Task GetScheduleDataAsync()
        {
            if (!IsLoaded)
            {
                await LoadAsync();
                CreateIndexes();
                // Fire a SchedulesLoaded event
                if (SchedulesLoaded != null)
                    SchedulesLoaded(this, null);

                IsLoaded = true;
            }
        }

        private async Task LoadAsync()
        {
            var wednesdayJsonText = await FileIO.ReadTextAsync(await StorageFile.GetFileFromApplicationUriAsync(wednesdayUri));
            var thrusdayJsonText = await FileIO.ReadTextAsync(await StorageFile.GetFileFromApplicationUriAsync(thursdayUri));
            var fridayJsonText = await FileIO.ReadTextAsync(await StorageFile.GetFileFromApplicationUriAsync(fridayUri));

            schedules.Clear();
            schedules.Add(Utils.DeserializeSchedule(wednesdayJsonText));
            schedules.Add(Utils.DeserializeSchedule(thrusdayJsonText));
            schedules.Add(Utils.DeserializeSchedule(fridayJsonText));          
        }

        private static void CreateIndexes()
        {
            _scheduleDataSource.hoursIndex.Clear();
            _scheduleDataSource.roomsIndex.Clear();
            foreach (var schedule in _scheduleDataSource.schedules)
            {
                _scheduleDataSource.hoursIndex.Add(Utils.CreateIndex(schedule, HourCriteria()));
                _scheduleDataSource.roomsIndex.Add(Utils.CreateIndex(schedule, RoomCriteria()));
            }
        }

        public static async Task RefreshAsync()
        {
            IEnumerable<string> pathes = new List<string>
                {
                    "http://cfp.devoxx.fr/api/conferences/DevoxxFR2015/schedules/wednesday/",
                    "http://cfp.devoxx.fr/api/conferences/DevoxxFR2015/schedules/thursday/",
                    "http://cfp.devoxx.fr/api/conferences/DevoxxFR2015/schedules/friday/"
                };
            string jsonText;
            _scheduleDataSource.Schedules.Clear();
            using (var client = new HttpClient())
            {
                foreach (var path in pathes)
                {
                    var response = await client.GetAsync(path);
                    jsonText = await response.Content.ReadAsStringAsync();
                    var schedule = Utils.DeserializeSchedule(jsonText);
                    _scheduleDataSource.Schedules.Add(schedule);
                }
            }
            CreateIndexes();
            SaveAsync();
        }

        public static IEnumerable<Slot> GetScheduleOfHourAsync(string day, string hour)
        {
            var schedule = _scheduleDataSource.Schedules.FirstOrDefault(s => s.Day == day);
            var slots = schedule.Slots;

            return slots.Where(s => s.FromTimeToTime == hour);
        }

        public static IEnumerable<Slot> GetScheduleOfRoomAsync(string day, string room)
        {
            var schedule = _scheduleDataSource.Schedules.FirstOrDefault(s => s.Day == day);
            var slots = schedule.Slots;

            return slots.Where(s => s.RoomName == room);
        }

        private static void SaveAsync()
        {
            SaveSchedule(wednesdayUri, "wednesday");
            SaveSchedule(thursdayUri, "thursday");
            SaveSchedule(fridayUri, "friday");
        }

        private static async Task SaveSchedule(Uri targetUri, string day)
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(targetUri);
            var text = Utils.SerizalizeSchedule(_scheduleDataSource.schedules.FirstOrDefault(s => s.Day == day));
            FileIO.WriteTextAsync(file, text, UnicodeEncoding.Utf8);
        }

        private static Func<Slot, string> RoomCriteria()
        {
            return s => s.RoomName;
        }

        private static Func<Slot, string> HourCriteria()
        {
            return s => s.FromTimeToTime;
        }
    }
}
