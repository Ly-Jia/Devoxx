using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

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
    }
}
