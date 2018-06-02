namespace Playground.Domain
{
    public class SearchBase
    {
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
