using System.Runtime.Serialization;

namespace Devoxx.Model
{
    [DataContract]
    public class Speaker
    {
        public Speaker(string name, Link link)
        {
            Name = name;
            Link = link;
        }

        // "name": "José Paumard"
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name="link")]
        public Link Link { get; set; }

    }
}
