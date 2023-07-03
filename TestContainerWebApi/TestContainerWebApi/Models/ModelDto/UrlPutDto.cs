using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestContainerWebApi.Models.ModelDto
{
    public class UrlPutDto
    {
        [DisplayName("NewURL")]
        public string NewUrl { get; set; }

        [Required(ErrorMessage = "Secret Access Token of URL not set")]
        public Guid SecretAccessToken { get; set; }
    }
}
