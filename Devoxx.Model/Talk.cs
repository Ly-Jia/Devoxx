using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Devoxx.Model
{
    [DataContract]
    public class Talk
    {
        //"id": "DNY-501"
        public Talk(string id, string title, string talkType, string track, string summary, string summaryAsHtml, string lang, IEnumerable<Speaker> speakers)
        {
            Id = id;
            Title = title;
            TalkType = talkType;
            Track = track;
            Summary = summary;
            SummaryAsHtml = summaryAsHtml;
            Lang = lang;
            Speakers = speakers;
        }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        //"title": "Java 8 Streams & Collectors : patterns, performances, parallélisation"
        [DataMember(Name="title")]
        public string Title { get; set; }

        //"talkType": "University"
        [DataMember(Name="talkType")]
        public string TalkType { get; set; }

        //"track": "Java SE, Java EE",
        [DataMember(Name = "track")]
        public string Track { get; set; }

        //"summary": "Java 8 est la, on en parle au présent. La nouveauté majeure de Java 8 est bien sûr l'introduction des lambda expressions. Cette introduction n'aurait pas été utile si l'API Collection n'avait été revue. Cette révision s'appelle l'API Stream, qui, ajoutée à la notion de Collector, rend un peu vieillot notre bon vieux pattern Iterator. Cette université se propose de présenter les patterns construits sur les Streams, et de montrer sur de nombreux exemples, simples et complexes, les nouveaux patterns proposés par cette API. On parlera patterns, parallélisation, implémentation et performances. "
        [DataMember(Name="summary")]
        public string Summary { get; set; }

        // "summaryAsHtml": "Java 8 est la, on en parle au présent. La nouveauté majeure de Java 8 est bien sûr l'introduction des lambda expressions. Cette introduction n'aurait pas été utile si l'API Collection n'avait été revue. Cette révision s'appelle l'API Stream, qui, ajoutée à la notion de Collector, rend un peu vieillot notre bon vieux pattern Iterator. Cette université se propose de présenter les patterns construits sur les Streams, et de montrer sur de nombreux exemples, simples et complexes, les nouveaux patterns proposés par cette API. On parlera patterns, parallélisation, implémentation et performances.\n",
        [DataMember(Name = "summaryAsHtml")]
        public string SummaryAsHtml { get; set; }

        //"lang": "fr"
        [DataMember(Name = "lang")]
        public string Lang { get; set; }

        [DataMember(Name = "speakers")]
        public IEnumerable<Speaker> Speakers { get; set; } 
    }
}
