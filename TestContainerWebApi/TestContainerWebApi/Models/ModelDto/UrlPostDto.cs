using System.ComponentModel.DataAnnotations;

namespace TestContainerWebApi.Models.ModelDto
{
    public class UrlPostDto
    {
        [Required(ErrorMessage = "Original URL not set")]
        public string OriginalUrl { get; set; }
    }
}
