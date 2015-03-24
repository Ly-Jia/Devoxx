using System.Runtime.Serialization;

namespace Devoxx.Model
{
    [DataContract]
    public class Link
    {
        public Link(string href, string rel, string title)
        {
            HRef = href;
            Rel = rel;
            Title = title;
        }

        //"href": "http://cfp.devoxx.fr/api/conferences/DevoxxFR2015/speakers/70365c89d2a734da0d24d091f7ec0af77ba90701"
        [DataMember(Name = "href")]
        public string HRef { get; set; }

        //"rel": "http://cfp.devoxx.fr/api/profile/speaker"
        [DataMember(Name = "Rel")]
        public string Rel { get; set; }

        //"title": "José Paumard"
        [DataMember(Name="title")]
        public string Title { get; set; }
    }
}
