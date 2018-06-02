namespace Playground.Domain
{
    public class ItemSearchParameter
    {
        public string Name { get; set; }
        public string Owner { get; set; }
        public string Tag { get; set; }

        private const int DefaultLimit = 100;

        private int? _skip;

        public int Skip
        {
            get { return _skip.GetValueOrDefault(0); }
            set { _skip = value; }
        }

        private int? _limit;

        public int Limit
        {
            get { return _limit.GetValueOrDefault(DefaultLimit); }
            set { _limit = value; }
        }
    }
}
