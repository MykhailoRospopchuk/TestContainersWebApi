using Microsoft.AspNetCore.Mvc;
using TestContainerWebApi.db;
using TestContainerWebApi.Models.ModelDto;
using TestContainerWebApi.Models;
using TestContainerWebApi.Services;
using Microsoft.AspNetCore.Authorization;

namespace TestContainerWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlUserController : ControllerBase
    {
        private readonly AdoDbContext _dbContext;

        public UrlUserController(AdoDbContext db_context)
        {
            _dbContext = db_context;
        }

        // GET: api/<UrlUserController>/get-all
        [HttpGet("get-all")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult<List<Url>>> GetAll()
        {
            try
            {
                string userEmail = User.Identity.Name;

                UserDto currentUser = await _dbContext.GetUserByEmail(userEmail);

                if (currentUser == null)
                {
                    return Unauthorized("Access denied.  User is not registered.");
                }

                List<Url> result = new List<Url>();
                result = await _dbContext.GetAllUrlByUser(currentUser.Id);
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

        // GET api/<UrlUserController>/get-one/5
        [HttpGet("get-one/{id}")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult<Url>> Get(int id)
        {
            try
            {
                string userEmail = User.Identity.Name;

                UserDto currentUser = await _dbContext.GetUserByEmail(userEmail);

                var result = await _dbContext.GetUrlByUser(id, currentUser.Id);
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

        // POST api/<UrlUserController>/create
        [HttpPost("create")]
        public async Task<ActionResult<int>> Post([FromBody] UrlPostDto urlIncome)
        {
            bool condition = true;
            int url_id;
            string short_code = "";
            int creatorId = 0;
            string originalUrl = urlIncome.OriginalUrl;

            string userEmail = User.Identity.Name;

            UserDto currentUser = await _dbContext.GetUserByEmail(userEmail);

            if (currentUser != null)
            {
                creatorId = currentUser.Id;
            }
            
            try
            {
                UserDto check_user = await _dbContext.GetUser(creatorId);
                if (check_user == null)
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

        // PUT api/<UrlUserController>/update
        [HttpPut("update")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult<Url>> Put([FromBody] UrlPutDto urlIncome)
        {
            string userEmail = User.Identity.Name;

            UserDto currentUser = await _dbContext.GetUserByEmail(userEmail);

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
                if (urlToUpdate.CreatorId != currentUser.Id)
                {
                    return Unauthorized("Access denied. User is not authorize.");
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

        // DELETE api/<UrlUserController>/delete
        [HttpDelete("delete")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult<Url>> Delete([FromBody] DeleteUrlDto urlIncome)
        {
            string userEmail = User.Identity.Name;

            UserDto currentUser = await _dbContext.GetUserByEmail(userEmail);

            Guid secretAccessToken = urlIncome.SecretAccessToken;

            try
            {
                Url urlToDelete = await _dbContext.GetUrlBySecret(secretAccessToken);

                if (urlToDelete.IsUrlDeleted())
                {
                    return Ok(urlToDelete);
                }
                if (urlToDelete == null)
                {
                    return NotFound($"URL with toke: {secretAccessToken} not exist");
                }

                if (urlToDelete.CreatorId != currentUser.Id)
                {
                    return Unauthorized("Access denied. User is not authorize.");
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

        // GET api/<UrlUserController>/v1/{secretAccessToken}/stats.json
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
                return Ok(new { views_stats = result });
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e}");
            }
        }
    }
}
