using System.Runtime.Serialization;

namespace Devoxx.Model
{
    [DataContract]
    public class Break
    {
        public Break(string id, Room room, string nameEn, string nameFr) 
        {
            Id = id;
            NameEN = nameEn;
            NameFR = nameFr;
            Room = room;
        }

        //"id":"coffee"
        [DataMember(Name="id")]
        public string Id { get; set; }

        //"nameEN":"Coffee Break"
        [DataMember(Name="nameEN")]
        public string NameEN { get; set; }

        //"nameFR":"Pause café"
        [DataMember(Name = "nameFR")]
        public string NameFR { get; set; }

        [DataMember(Name="room")]
        public Room Room { get; set; }
    }
}
