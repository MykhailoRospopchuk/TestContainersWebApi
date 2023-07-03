using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TestContainerWebApi.db;
using TestContainerWebApi.Models;
using TestContainerWebApi.Models.ModelDto;
using TestContainerWebApi.Services;


namespace TestContainerWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class UrlAdminController : ControllerBase
    {
        private readonly AdoDbContext _dbContext;

        public UrlAdminController(AdoDbContext db_context)
        {
            _dbContext = db_context;
        }

        // GET: api/<UrlAdminController>/get-all
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

        // GET api/<UrlAdminController>/get-one/5
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

        // POST api/<UrlAdminController>/create
        [HttpPost("create")]
        public async Task<ActionResult<Url>> Post([FromBody] UrlPostDto urlIncome)
        {
            bool condition = true;
            string short_code = "";
            string originalUrl = urlIncome.OriginalUrl;
            int creatorId = 1;
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
                var result = await _dbContext.CreateUrl(originalUrl, short_code, token, creatorId);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e}");
            }
        }

        // PUT api/<UrlAdminController>/update
        [HttpPut("update")]
        public async Task<ActionResult<Url>> Put([FromBody] UrlPutDto urlIncome)
        {
            string newUrl = urlIncome.NewUrl;
            Guid secretAccessToken = urlIncome.SecretAccessToken;

            try
            {
                Url urlToUpdate = await _dbContext.GetUrlBySecret(secretAccessToken);
                if (urlToUpdate == null)
                {
                    return NotFound($"URL with toke: {secretAccessToken} not exist");
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

        // DELETE api/<UrlAdminController>/delete
        [HttpDelete("delete")]
        public async Task<ActionResult<Url>> Delete([FromBody] DeleteUrlDto urlIncome)
        {
            Guid secretAccessToken = urlIncome.SecretAccessToken;

            try
            {
                Url urlToDelete = await _dbContext.GetUrlBySecret(secretAccessToken);
                if (urlToDelete == null)
                {
                    return NotFound($"URL with toke: {secretAccessToken} not exist");
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

        // GET api/<UrlAdminController>/v1/{secretAccessToken}/stats.json
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
