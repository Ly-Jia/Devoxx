using Devoxx.Model;

namespace Test.Devoxx.Model.Builders
{
    public class ARoom
    {
        private string id;
        private string name;
        private int capacity;
        private string setup;

        public ARoom(string roomName)
        {
            name = roomName;
        }

        public Room Build()
        {
            return new Room(id, name, capacity, setup);
        }

    }
}
