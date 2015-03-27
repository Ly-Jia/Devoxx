namespace Devoxx.Model
{
    public class IndexValue
    {
        public string Value { get; set; }
        public string Key { get; set; }

        public IndexValue(string value, string key)
        {
            Value = value;
            Key = key;
        }
    }
}
