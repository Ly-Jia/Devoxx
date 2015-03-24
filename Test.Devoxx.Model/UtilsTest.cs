using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Devoxx.Model;
using NFluent;
using NUnit.Framework;

namespace Test.Devoxx.Model
{
    [TestFixture]
    public class UtilsTest
    {
        private const string RoomId = "a_hall";
        private const string BreakId = "dej";
        private const string BreakNameEn = "Breakfast";
        private const string BreakNameFr = "Accueil et petit-déjeuner";
        private const string RoomName = "Exhibition floor";
        private const int RoomCapacity = 1500;
        private const string FromTime = "08:00";
        private const string ToTime = "09:30";
        private const string SlotId = "dej_thursday_13_8h0_8h30";
        private const string Day = "thursday";
        private const string RoomSetup = "special";
        private const string SpeakerName = "John Doe";
        private const string LinkHRef = "http//www.johndoe.com";
        private const string LinkRel = "rel";
        private const string LinkTitle = "John Doe's profile";
        private const string TalkId = "JOHN_DOE_DOTNET";
        private const string TalkTitle = ".NET for dumbs";
        private const string TalkType = "Conference";
        private const string TalkTrack = "Cool stuff";
        private const string TalkSummary = ".NET rocks";
        private const string TalkSummaryAsHtml = @".NET rocks \n";
        private const string TalkLang = "EN";

        [Test]
        public void should_add_a_break_from_stream()
        {
            var room = new Room(RoomId, RoomName, RoomCapacity, RoomSetup);
            var aBreak = new Break(BreakId, room, BreakNameEn, BreakNameFr);
            var slots = new ObservableCollection<Slot> { new Slot(SlotId, FromTime, ToTime, RoomId, RoomName, RoomCapacity, RoomSetup, aBreak, null) };
            var expectedSchedule = new Schedule(Day, slots);

            var jsonText = CreateScheduleJsonText(FromTime, ToTime, SlotId, Day, room, CreateBreak(aBreak)); 
            var schedule = Utils.DeserializeSchedule(jsonText);

            Check.That(schedule).HasFieldsWithSameValues(expectedSchedule);
        }

        [Test]
        public void should_add_a_talk_from_stream()
        {
            var room = new Room(RoomId, RoomName, RoomCapacity, RoomSetup);
            var speaker = new Speaker(SpeakerName, new Link(LinkHRef, LinkRel, LinkTitle));
            var talk = new Talk(TalkId, TalkTitle, TalkType, TalkTrack, TalkSummary, TalkSummaryAsHtml, TalkLang,
                new List<Speaker>{speaker});
            var slots = new ObservableCollection<Slot> { new Slot(SlotId, FromTime, ToTime, RoomId, RoomName, RoomCapacity, RoomSetup, null, talk) };
            var expectedSchedule = new Schedule(Day, slots);

            var jsonText = CreateScheduleJsonText(FromTime, ToTime, SlotId, Day, room, CreateTalk(talk));
            var schedule = Utils.DeserializeSchedule(jsonText);

            Check.That(schedule).HasFieldsWithSameValues(expectedSchedule);
        }

        private string CreateScheduleJsonText(string fromTime, string toTime, string slotId, string day, Room room, string talkOrBreak)
        {
            return "{\"slots\":[{\"roomId\":\"" + room.Id + "\",\"notAllocated\":false,\"fromTimeMillis\":1415862000000," 
                    + talkOrBreak +
                   "\"roomSetup\":\""+room.Setup+"\","+
                   "\"fromTime\":\"" + fromTime + "\",\"toTimeMillis\":1415867400000,\"toTime\":\"" + toTime +
                   "\",\"roomCapacity\":" + room.Capacity + ",\"roomName\":\"" + room.Name + "\"," +
                   "\"slotId\":\"" + slotId + "\",\"day\":\"" + day + "\"}]} ";
        }

        private string CreateBreak(Break aBreak)
        {
            return "\"break\":{\"id\":\"" + aBreak.Id + "\",\"nameEN\":\"" + aBreak.NameEN + "\",\"nameFR\":\"" + aBreak.NameFR + "\"," +
                   "\"room\":{\"id\":\"" + aBreak.Room.Id + "\",\"name\":\"" + aBreak.Room.Name + "\",\"capacity\":" + aBreak.Room.Capacity +
                   ",\"setup\":\""+aBreak.Room.Setup+"\"}}," + "\"talk\":null,";
        }

        private string CreateTalk(Talk talk)
        {
            var speaker = talk.Speakers.First();
            var link = speaker.Link;
            return "\"talk\": {" +
                                    "\"talkType\": \""+talk.TalkType+"\"," +
                                    "\"track\": \""+talk.Track+"\"," +
                                    "\"summaryAsHtml\": \""+talk.SummaryAsHtml+"\","+
                                    "\"id\": \""+talk.Id+"\"," +
                                    "\"speakers\": [ {" +
                                                    "\"link\": {" +
                                                        "\"href\": \""+link.HRef+"\","+
                                                        "\"rel\": \""+link.Rel+"\"," +
                                                        "\"title\": \""+link.Title+"\"},"+
                                                    "\"name\": \""+speaker.Name+"\"}]," +
                                    "\"title\": \""+talk.Title+"\"," +
                                    "\"lang\": \""+talk.Lang+"\"," +
                                    "\"summary\": \""+talk.Summary+" \"}, ";
        }
    }
}
