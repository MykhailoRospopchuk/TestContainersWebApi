namespace TestContainerWebApi.Models
{
    public class View
    {
        private readonly int _urlId;
        private readonly DateTime _viewAt;
        private readonly int _count;

        public View(int urlId, DateTime viewAt, int count)
        {
            _urlId = urlId;
            _viewAt = viewAt;
            _count = count;
        }

        public int UrlId { get { return _urlId; } }
        public DateTimeOffset ViewAt { get { return _viewAt; } }
        public int Count { get { return _count; } }
    }
}
