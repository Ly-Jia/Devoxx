using Devoxx.Model;
using NFluent;
using NUnit.Framework;
using Test.Devoxx.Model.Builders;

namespace Test.Devoxx.Model
{
    [TestFixture]
    public class SlotTest
    {
        [Test]
        public void roomName_should_be_name_of_room_from_break_when_slot_is_break()
        {
            var amphi = "amphi";
            var slot = new ASlot().ThatIs(new ABreak().TakingPlaceIn(new ARoom(amphi).Build()).Build()).Build();

            Check.That(slot.RoomName).Equals(amphi);
        }

        [Test]
        public void roomName_should_be_name_of_room_from_slot_when_slot_is_not_break()
        {
            var amphi = "amphi";
            var slot = new ASlot().TakingPlaceInRoom(amphi).Build();

            Check.That(slot.RoomName).Equals(amphi);
        }

        [Test]
        public void roomName_should_be_empty_when_break_has_no_room()
        {
            var slot = ASlot.EmptyBreak;

            Check.That(slot.RoomName).IsEmpty();
        }

        [Test]
        public void roomName_should_be_empty_when_no_room_is_provided()
        {
            var slot = new ASlot().Build();

            Check.That(slot.RoomName).IsEmpty();
        }

        [Test]
        public void title_should_be_talk_title_when_slot_is_talk()
        {
            var title = "title";

            var slot = new ASlot().ThatIs(new ATalk(title).Build()).Build();

            Check.That(slot.Title).Equals(title);
        }

        [Test]
        public void title_should_be_empty_when_slot_is_talk_and_talk_title_is_not_provided()
        {
            var slot = ASlot.EmptyTalk;

            Check.That(slot.Title).IsEmpty();
        }

        [Test]
        public void title_should_be_break_name_in_french_when_slot_is_break()
        {
            var nameInFrench = "pause";

            var slot = new ASlot().ThatIs(new ABreak().WithNameFr(nameInFrench).Build()).Build();

            Check.That(slot.Title).Equals(nameInFrench);
        }

        [Test]
        public void title_should_be_break_name_in_english_when_slot_is_break_and_name_in_french_is_not_provided()
        {
            var nameInEnglish = "rest";

            var slot = new ASlot().ThatIs(new ABreak().WithNameEn(nameInEnglish).Build()).Build();

            Check.That(slot.Title).Equals(nameInEnglish);
        }

        [Test]
        public void title_should_be_empty_when_slot_is_break_and_no_name_is_provided()
        {
            var slot = ASlot.EmptyBreak;

            Check.That(slot.Title).IsEmpty();
        }

        [Test]
        public void type_should_be_break_when_slot_is_break()
        {
            var slot = ASlot.EmptyBreak;
            
            Check.That(slot.Type).Equals("Break");
        }

        [Test]
        public void type_should_be_talk_type_when_slot_is_talk()
        {
            var talkType = "talkType";

            var slot = new ASlot().ThatIs(new ATalk().OfType(talkType).Build()).Build();

            Check.That(slot.Type).Equals(talkType);
        }

        [Test]
        public void type_should_be_empty_when_slot_is_talk_and_talk_type_is_not_provided()
        {
            var slot = ASlot.EmptyTalk;

            Check.That(slot.Type).IsEmpty();
        }

        [Test]
        public void track_should_be_empty_when_slot_is_break()
        {
            var slot = ASlot.EmptyBreak;

            Check.That(slot.Track).IsEmpty();
        }

        [Test]
        public void track_should_be_talk_track_when_slot_is_talk()
        {
            var track = "track";

            var slot = new ASlot().ThatIs(new ATalk().OnTrack(track).Build()).Build();

            Check.That(slot.Track).Equals(track);
        }

        [Test]
        public void track_should_be_empty_when_slot_is_talk_and_talk_type_is_not_provided()
        {
            var slot = ASlot.EmptyTalk;

            Check.That(slot.Track).IsEmpty();
        }

        [Test]
        public void summary_should_be_empty_when_slot_is_break()
        {
            var slot = ASlot.EmptyBreak;

            Check.That(slot.Summary).IsEmpty();
        }

        [Test]
        public void summary_should_be_talk_summary_when_slot_is_talk()
        {
            var summary = "summary";

            var slot = new ASlot().ThatIs(new ATalk().About(summary).Build()).Build();

            Check.That(slot.Summary).Equals(summary);
        }

        [Test]
        public void summary_should_be_empty_when_slot_is_talk_and_summary_is_not_provided()
        {
            var slot = ASlot.EmptyTalk;

            Check.That(slot.Summary).IsEmpty();
        }
    }

}
    