using Microsoft.AspNetCore.Mvc;
using TestContainerWebApi.db;
using TestContainerWebApi.Models;

namespace TestContainerWebApi.Controllers
{
    [ApiController]
    public class RedirectController : ControllerBase
    {
        private readonly AdoDbContext _dbContext;

        public RedirectController(AdoDbContext db_context)
        {
            _dbContext = db_context;
        }


        // GET: /shortUrl
        [HttpGet("{shortUrl}")]
        public async Task<IActionResult> RedirectToOriginal(string shortUrl)
        {
            try
            {
                Url urlToRedirect = await _dbContext.GetUrlByShort(shortUrl);
                if (urlToRedirect == null)
                {
                    return NotFound();
                }

                
                if (urlToRedirect.IsUrlDeleted())
                {
                    return NotFound("This short URL has been removed");
                }


                View urlView = await _dbContext.CreateView(urlToRedirect.Id);

                if (urlView == null)
                {
                    return NotFound("This short URL has problem with statistic");
                }

                return Redirect(urlToRedirect.OriginalUrl);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e}");
            }
        }
    }
}
