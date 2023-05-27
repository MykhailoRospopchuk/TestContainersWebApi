namespace TestContainerWebApi.Models
{
    public class Url
    {
        private readonly int _id;
        private string _original_url;
        private readonly string _short_url;
        private Guid _secret_access_token;
        private DateTimeOffset _created_at;
        private DateTimeOffset? _updated_at;
        private DateTimeOffset? _deleted_at;
        private readonly int _creator_id;

        public Url(int id, string original_url, string short_url, Guid secret_access_token, DateTimeOffset created_at, int creator_id, DateTimeOffset? updated_at = null,  DateTimeOffset? deleted_at = null)
        {
            _id = id;
            _original_url = original_url;
            _short_url = short_url;
            _secret_access_token = secret_access_token;
            _created_at = created_at;
            _updated_at = updated_at;
            _deleted_at = deleted_at;
            _creator_id = creator_id;
        }

        public int Id { get { return _id; } }
        public string OriginalUrl { get { return _original_url; } }
        public string ShortUrl { get { return _short_url; } }
        public Guid AccessToken { get { return _secret_access_token; } }
        public DateTimeOffset CreatedAt { get { return _created_at; } }
        public DateTimeOffset? Updated_at { get { return _updated_at; } }
        public DateTimeOffset? Deleted_at { get { return _deleted_at; } }
        public int CreatorId { get { return _creator_id; } }

        public void UpdateUrl(string new_original_url = "")
        {
            if (new_original_url != "")
            {
                _original_url = new_original_url;
            }
            _updated_at = new DateTimeOffset(DateTime.Now);
        }

        public void DeleteUrl()
        {
            _deleted_at = new DateTimeOffset(DateTime.Now);
        }

        public bool IsUrlDeleted()
        {
            return _deleted_at != null;
        }
    }
}
