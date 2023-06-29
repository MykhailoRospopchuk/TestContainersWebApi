using System.ComponentModel.DataAnnotations;

namespace TestContainerWebApi.Models.ModelDto
{
    public class DeleteUrlDto
    {
        [Required(ErrorMessage = "Id of URL not set")]
        public int UrlId { get; set; }

        [Required(ErrorMessage = "Secret Access Token of URL not set")]
        public Guid SecretAccessToken { get; set; }
    }
}
