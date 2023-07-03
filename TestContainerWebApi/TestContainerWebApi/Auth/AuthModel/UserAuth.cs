using System.Data;

namespace TestContainerWebApi.Auth.AuthModel
{
    public class UserAuth
    {

        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public int? RoleId { get; set; }
        public RoleAuth Role { get; set; }

    }
}
