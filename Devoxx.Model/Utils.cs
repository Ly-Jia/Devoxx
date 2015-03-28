using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Devoxx.Model
{
    public class Utils
    {
        public static Schedule DeserializeSchedule(string sheduleText)
        {
            List<Type> types = new List<Type>();
            types.Add(typeof(Slot));
            types.Add(typeof(Break));
            types.Add(typeof(Talk));
            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(Schedule), types);
            var stream = new MemoryStream(Encoding.Unicode.GetBytes(sheduleText));
            var schedule = (Schedule)deserializer.ReadObject(stream);
            schedule.Day = schedule.Slots.First().Day;
           
            return schedule;
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
