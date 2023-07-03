using Microsoft.AspNetCore.Mvc;
using TestContainerWebApi.db;
using TestContainerWebApi.Models;
using TestContainerWebApi.Models.ModelDto;
using TestContainerWebApi.Services;


namespace TestContainerWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlController : ControllerBase
    {
        private readonly AdoDbContext _dbContext;

        public UrlController(AdoDbContext db_context)
        {
            _dbContext = db_context;
        }

        // GET: api/<UrlController>/get-all
        [HttpGet("get-all")]
        public async Task<ActionResult<List<Url>>> GetAll()
        {
            try
            {
                List<Url> result = new List<Url>();
                result = await _dbContext.GetAllUrl();
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e}");
            }
        }

        // GET api/<UrlController>/get-one/5
        [HttpGet("get-one/{id}")]
        public async Task<ActionResult<Url>> Get(int id)
        {
            try
            {
                var result = await _dbContext.GetUrl(id);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e}");
            }
        }

        // POST api/<UrlController>/create
        [HttpPost("create")]
        public async Task<ActionResult<int>> Post([FromBody] UrlPostDto urlIncome)
        {
            bool condition = true;
            int url_id;
            string short_code = "";
            string originalUrl = urlIncome.OriginalUrl;
            int creatorId = urlIncome.CreatorId;
            try
            {
                UserDto check_user = await _dbContext.GetUser(creatorId);
                if(check_user == null)
                {
                    return NotFound($"User with ID: {creatorId} not exist");
                }

                while (condition)
                {
                    short_code = GenerateShort.GenerateShortUrl();
                    Url is_exist = await _dbContext.GetUrlByShort(short_code);
                    if (is_exist == null)
                    {
                        condition = false;
                    }
                }

                Guid token = AccessTokenUrl.GenerateAccessToken();
                url_id = await _dbContext.CreateUrl(originalUrl, short_code, token, creatorId);
                return Ok(url_id);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e}");
            }
        }

        // PUT api/<UrlController>/update
        [HttpPut("update")]
        public async Task<ActionResult<Url>> Put([FromBody] UrlPutDto urlIncome)
        {
            string newUrl = urlIncome.NewUrl;
            int urlId = urlIncome.UrlId;
            Guid secretAccessToken = urlIncome.SecretAccessToken;

            try
            {
                Url urlToUpdate = await _dbContext.GetUrl(urlId);
                if (urlToUpdate == null)
                {
                    return NotFound($"URL with ID: {urlId} not exist");
                }
                if (secretAccessToken != urlToUpdate.AccessToken)
                {
                    return BadRequest($"Wrong Secret Access Token");
                }

                urlToUpdate.UpdateUrl(newUrl);

                var result = await _dbContext.UpdateUrl(urlToUpdate.Id, urlToUpdate.OriginalUrl, urlToUpdate.AccessToken, urlToUpdate.UpdatedAt);

                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e}");
            }
        }

        // DELETE api/<UrlController>/delete
        [HttpDelete("delete")]
        public async Task<ActionResult<Url>> Delete([FromBody] DeleteUrlDto urlIncome)
        {
            int urlId = urlIncome.UrlId;
            Guid secretAccessToken = urlIncome.SecretAccessToken;

            try
            {
                Url urlToDelete = await _dbContext.GetUrl(urlId);
                if (urlToDelete == null)
                {
                    return NotFound($"URL with ID: {urlId} not exist");
                }
                if (secretAccessToken != urlToDelete.AccessToken)
                {
                    return BadRequest("Wrong Secret Access Token");
                }

                if (urlToDelete.IsUrlDeleted())
                {
                    return Ok(urlToDelete);
                }

                urlToDelete.DeleteUrl();

                var result = await _dbContext.DeleteUrl(urlToDelete.Id, urlToDelete.AccessToken, urlToDelete.DeletedAt);

                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e}");
            }
        }

        // GET api/<UrlController>/v1/{secretAccessToken}/stats.json
        [HttpGet("v1/{secretAccessToken}/stats.json")]
        public async Task<IActionResult> GetStats(Guid secretAccessToken)
        {
            try
            {
                List<ViewStat> result = new List<ViewStat>();

                result = await _dbContext.GetViewStats(secretAccessToken);
                if (result == null)
                {
                    return BadRequest("Wrong Secret Access Token");
                }
                return Ok( new { views_stats = result });
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e}");
            }
        }
    }
}
