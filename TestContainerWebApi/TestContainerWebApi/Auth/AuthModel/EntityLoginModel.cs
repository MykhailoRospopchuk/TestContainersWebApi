using System.ComponentModel.DataAnnotations;

namespace TestContainerWebApi.Auth.AuthModel
{
    public class EntityLoginModel
    {
        [Required(ErrorMessage = "Email not set")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password not set")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
