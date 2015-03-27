using System.Collections.Generic;

namespace Devoxx.Model
{
    public class Index
    {
        public string Key { get; set; }
        public IList<IndexValue> Hours { get; set; }

        public Index(string key, IList<IndexValue> hours)
        {
            Key = key;
            Hours = hours;
        }
    }
}
