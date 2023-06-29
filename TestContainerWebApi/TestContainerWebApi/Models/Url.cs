namespace TestContainerWebApi.Models
{
    public class Url
    {
        private readonly int _id;
        private string _originalUrl;
        private readonly string _shortUrl;
        private Guid _secretAccessToken;
        private DateTimeOffset _createdAt;
        private DateTimeOffset? _updatedAt;
        private DateTimeOffset? _deletedAt;
        private readonly int _creatorId;

        public Url(int id, string originalUrl, string shortUrl, Guid secretAccessToken, DateTimeOffset createdAt, int creatorId, DateTimeOffset? updatedAt = null,  DateTimeOffset? deletedAt = null)
        {
            _id = id;
            _originalUrl = originalUrl;
            _shortUrl = shortUrl;
            _secretAccessToken = secretAccessToken;
            _createdAt = createdAt;
            _updatedAt = updatedAt;
            _deletedAt = deletedAt;
            _creatorId = creatorId;
        }

        public int Id { get { return _id; } }
        public string OriginalUrl { get { return _originalUrl; } }
        public string ShortUrl { get { return _shortUrl; } }
        public Guid AccessToken { get { return _secretAccessToken; } }
        public DateTimeOffset CreatedAt { get { return _createdAt; } }
        public DateTimeOffset? UpdatedAt { get { return _updatedAt; } }
        public DateTimeOffset? DeletedAt { get { return _deletedAt; } }
        public int CreatorId { get { return _creatorId; } }

        public void UpdateUrl(string new_original_url = "")
        {
            if (new_original_url != "")
            {
                _originalUrl = new_original_url;
            }
            _updatedAt = new DateTimeOffset(DateTime.Now);
        }

        public void DeleteUrl()
        {
            _deletedAt = new DateTimeOffset(DateTime.Now);
        }

        public bool IsUrlDeleted()
        {
            return _deletedAt != null;
        }
    }
}
