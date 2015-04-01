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

        private static DataContractJsonSerializer agendaSerializer = new DataContractJsonSerializer(typeof (ObservableCollection<Slot>), types);
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

        public static ObservableCollection<Slot> DeserializeAgenda(string agendaText)
        {
            var stream = new MemoryStream(Encoding.Unicode.GetBytes(agendaText));
            var agenda = (ObservableCollection<Slot>) agendaSerializer.ReadObject(stream);
            return agenda;
        }

        public static string SerializeAgenda(IEnumerable<Slot> agenda)
        {
            using (var stream = new MemoryStream())
            {
                agendaSerializer.WriteObject(stream, agenda);
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
