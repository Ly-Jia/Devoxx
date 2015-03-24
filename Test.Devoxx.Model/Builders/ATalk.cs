using System.Collections.Generic;
using Devoxx.Model;

namespace Test.Devoxx.Model.Builders
{
    public class ATalk
    {
        private string id;
        private string title;
        private string talkType;
        private string track;
        private string summary;
        private string summaryAsHtml;
        private string lang;
        private IEnumerable<Speaker> speakers;

        public ATalk()
        {
        }

        public ATalk(string talkTitle)
        {
            title = talkTitle;
        }

        public ATalk OfType(string talkType)
        {
            this.talkType = talkType;
            return this;
        }

        public ATalk OnTrack(string track)
        {
            this.track = track;
            return this;
        }

        public ATalk About(string summary)
        {
            this.summary = summary;
            return this;
        }

        public Talk Build()
        {
            return new Talk(id, title, talkType, track, summary, summaryAsHtml, lang, speakers);
        }
    }
}
