using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Devoxx.Model
{
    [DataContract(Name = "root")]
    public class Schedule
    {

        public Schedule(string day, ObservableCollection<Slot> slots)
        {
            Day = day;
            Slots = slots;
        }

        [DataMember(Name = "slots")]
        public ObservableCollection<Slot> Slots { get; set; }

        public string Day { get; set; }
    }
}
