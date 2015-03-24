using System.Linq.Expressions;
using Devoxx.Model;

namespace Test.Devoxx.Model.Builders
{
    public class ASlot
    {
        public static readonly Slot EmptyBreak = new ASlot().ThatIs(new ABreak().Build()).Build();
        public static readonly Slot EmptyTalk = new ASlot().ThatIs(new ATalk().Build()).Build();
        private string id;
        private string fromTime;
        private string toTime;
        private string roomName;
        private string roomId;
        private int roomCapacity;
        private string roomSetup;
        private Break aBreak;
        private Talk talk;

        public ASlot()
        {
        }

        public ASlot ThatIs(Break aBreak)
        {
            this.aBreak = aBreak;
            return this;
        }

        public ASlot ThatIs(Talk talk)
        {
            this.talk = talk;
            return this;
        }

        public ASlot TakingPlaceInRoom(string rname)
        {
            roomName = rname;
            return this;
        }

        public Slot Build()
        {
            return new Slot(id, fromTime, toTime, roomId, roomName, roomCapacity, roomSetup, aBreak, talk);
        }

    }
}
