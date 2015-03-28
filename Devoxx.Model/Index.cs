using System.Collections.Generic;

namespace Devoxx.Model
{
    public class Index
    {
        public string Key { get; set; }
        public IList<IndexValue> Values { get; set; }

        public Index(string key, IList<IndexValue> values)
        {
            Key = key;
            Values = values;
        }
    }
}
