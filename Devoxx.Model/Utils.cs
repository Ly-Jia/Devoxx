using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Devoxx.Model
{
    public class Utils
    {
        private static IList<Type> types = new List<Type>
        {
            typeof (Slot),
            typeof (Break),
            typeof (Talk),
            typeof (Room),
            typeof (Speaker),
            typeof (Link),
            typeof(IEnumerable<Slot>)
        };

        private static DataContractJsonSerializer favouritesSerializer = new DataContractJsonSerializer(typeof (ObservableCollection<Slot>), types);
        private static DataContractJsonSerializer scheduleSerializer = new DataContractJsonSerializer(typeof(Schedule), types);

        public static Schedule DeserializeSchedule(string sheduleText)
        {
            var stream = new MemoryStream(Encoding.Unicode.GetBytes(sheduleText));
            return DeserializeSchedule(stream);
        }

        public static Schedule DeserializeSchedule(Stream stream)
        {
            var schedule = (Schedule) scheduleSerializer.ReadObject(stream);
            schedule.Day = schedule.Slots.First().Day;

            return schedule;
        }

        public static string SerizalizeSchedule(Schedule schedule)
        {
            using (var stream = new MemoryStream())
            {
                scheduleSerializer.WriteObject(stream, schedule);
                return StreamToByteArray(stream);
            }
        }

        public static ObservableCollection<Slot> DeserializeFavourites(string favouritesText)
        {
            var stream = new MemoryStream(Encoding.Unicode.GetBytes(favouritesText));
            var favourites = (ObservableCollection<Slot>) favouritesSerializer.ReadObject(stream);
            return favourites;
        }

        public static string SerializeFavourites(IEnumerable<Slot> favourites)
        {
            using (var stream = new MemoryStream())
            {
                favouritesSerializer.WriteObject(stream, favourites);
                return StreamToByteArray(stream);
            }
        }

        private static string StreamToByteArray(MemoryStream stream)
        {
            stream.Position = 0;
            var streamReader = new StreamReader(stream);
            var text = streamReader.ReadToEnd();
            return text;
            //return Encoding.Unicode.GetBytes(text);
        }

        public static Index CreateIndex(Schedule schedule, Func<Slot, string> indexColumn)
        {
            var index = new List<IndexValue>();
            var values = schedule.Slots.Select(indexColumn).Distinct();
            foreach (var value in values)
            {
                var indexValue = new IndexValue(value, schedule.Day);
                index.Add(indexValue);
            }
            var item = new Index(schedule.Day, index);
            return item;
        }
    }
}
