using System.ComponentModel.DataAnnotations;

namespace TestContainerWebApi.Models.ModelDto
{
    public class UrlResponseDto
    {
        [Required(ErrorMessage = "Original URL not set")]
        public string OriginalUrl { get; set; }

        [Required(ErrorMessage = "Short URL not set")]
        public string ShortUrl { get; set; }

        [Required(ErrorMessage = "Secret Access Token of URL not set")]
        public Guid SecretAccessToken { get; set; }
    }
}
