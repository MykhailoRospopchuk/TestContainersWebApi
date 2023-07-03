using System.ComponentModel.DataAnnotations;

namespace TestContainerWebApi.Auth.AuthModel
{
    public class EntityRegisterModel
    {
        [Required(ErrorMessage = "Email not set")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password not set")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Wrong Password")]
        public string ConfirmPassword { get; set; }
    }
}
