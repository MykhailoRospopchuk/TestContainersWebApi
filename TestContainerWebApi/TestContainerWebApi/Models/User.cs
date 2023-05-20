namespace TestContainerWebApi.Models
{
    public class User
    {
        private int _id;
        private DateTimeOffset _created_at;
        private DateTimeOffset _updated_at;

        public User(int id, DateTimeOffset created_at, DateTimeOffset updated_at)
        {
            _id = id;
            _created_at = created_at;
            _updated_at = updated_at;
        }

        public int Id { get { return _id; } }
        public DateTimeOffset CreatedAt { get {  return _created_at; } }
        public DateTimeOffset Updated_at { get { return _updated_at; } }

        public void Update_User()
        {
            _updated_at = DateTimeOffset.UtcNow;
        }
    }
}
