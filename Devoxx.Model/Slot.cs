using System;
using System.Runtime.Serialization;
using System.Text;

namespace Devoxx.Model
{
    [DataContract]
    public class Slot
    {
        private const string BreakType = "Break";

        public Slot(string id, string fromTime, string toTime, string roomId, string roomName, int roomCapacity, string roomSetup, Break _break, Talk talk)
        {
            Id = id;
            FromTime = fromTime;
            ToTime = toTime;
            RoomId = roomId;
            this.roomName = roomName;
            RoomCapacity = roomCapacity;
            RoomSetup = roomSetup;
            Break = _break;
            Talk = talk;
        }

        //"slotId":"coffee_thursday_13_10h30_9h50"
        [DataMember(Name = "slotId")]
        public string Id { get; set; }

        //"day":"thursday"
        [DataMember(Name = "day")]
        public string Day { get; set; }

        //"fromTime":"10:30"
        [DataMember(Name = "fromTime")]
        public string FromTime { get; set; }

        //"toTime":"10:50"
        [DataMember(Name = "toTime")]
        public string ToTime { get; set; }

        [DataMember(Name = "break")]
        public Break Break { get; set; }

        [DataMember(Name = "talk")]
        public Talk Talk { get; set; }

        [DataMember(Name = "roomId")]
        public string RoomId { get; set; }

        private string roomName;
        [DataMember(Name = "roomName")]
        public string RoomName
        {
            get
            {
                if (roomName != null)
                {
                    return roomName;
                }
                if (Break != null && Break.Room != null && Break.Room.Name!=null)
                {
                    return Break.Room.Name;
                }
                return String.Empty;
            }
            set { roomName = value; }
        }

        [DataMember(Name = "capacity")]
        public int RoomCapacity { get; set; }

        [DataMember(Name = "setup")]
        public string RoomSetup { get; set; }

        public string Title
        {
            get
            {
                if (Talk != null && Talk.Title != null) return Talk.Title;
                if (Break != null)
                {
                    if (Break.NameFR != null) return Break.NameFR;
                    if (Break.NameEN != null) return Break.NameEN;
                }
                return String.Empty;
            }
        }

        public string FromTimeToTime
        {
            get { return FromTime + "-" + ToTime; }
        }

        public string Type
        {
            get
            {
                if (Talk == null) return BreakType;
                if (Talk.TalkType == null) return String.Empty;
                return Talk.TalkType;
            }
        }

        public string Track
        {
            get
            {
                if (Talk == null || Talk.Track == null) return String.Empty;
                return Talk.Track;
            }
        }

        public string Summary
        {
            get
            {
                if (Talk == null || Talk.Summary == null) return String.Empty; 
                return Talk.Summary;
                
            }
        }

        public string Speakers
        {
            get
            {
                StringBuilder speakers = new StringBuilder();
                
                if (Talk != null)
                {
                    foreach (var speaker in Talk.Speakers)
                    {
                        speakers.Append(speaker.Name).Append(" ");
                    }
                    speakers.Append("(").Append(Talk.Lang).Append(")");
                }
                return speakers.ToString();
            }
        }

        public override bool Equals(object obj)
        {
            var slot2 = obj as Slot;
            return this.Id == slot2.Id;
        }
    }
}
