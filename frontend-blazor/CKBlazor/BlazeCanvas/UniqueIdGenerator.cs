namespace BlazeCanvas
{
    public class UniqueIdGenerator
    {
        string _namePrefix;
        int _nameCount;

        public UniqueIdGenerator(string prefix)
        {
            _namePrefix = prefix;
        }

        public string NextId()
        {
            _nameCount++;
            return $"{_namePrefix}-{_nameCount}";
        }
    }
}
