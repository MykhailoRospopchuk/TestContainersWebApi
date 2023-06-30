namespace TestContainerWebApi.Models
{
    public class ViewStat
    {
        private readonly DateTime _day;
        private readonly int _count;

        public ViewStat(DateTime day, int count)
        {
            _day = day;
            _count = count;
        }

        public DateTimeOffset Day { get { return _day; } }
        public int Count { get { return _count; } }
    }
}
