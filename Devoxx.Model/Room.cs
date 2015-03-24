using System.Runtime.Serialization;

namespace Devoxx.Model
{
    [DataContract]
    public class Room
    {
        public Room(string id, string name, int capicity, string setup)
        {
            Id = id;
            Name = name;
            Capacity = capicity;
            Setup = setup;
        }

        //"id":"a_hall"
        [DataMember(Name="id")]
        public string Id { get; set; }

        //"name":"Exhibition floor"
        [DataMember(Name = "name")]
        public string Name { get; set; }

        //"capacity":1500
        [DataMember(Name = "capacity")]
        public int Capacity { get; set; }

        //"setup":"special"
        [DataMember(Name="setup")]
        public string Setup { get; set; }
    }
}
