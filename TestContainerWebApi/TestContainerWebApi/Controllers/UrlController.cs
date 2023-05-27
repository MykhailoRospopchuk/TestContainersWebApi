using Microsoft.AspNetCore.Mvc;
using TestContainerWebApi.db;
using TestContainerWebApi.Models;
using TestContainerWebApi.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestContainerWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlController : ControllerBase
    {
        private readonly AdoDbContext _db_context;

        public UrlController(AdoDbContext db_context)
        {
            _db_context = db_context;
        }
        // GET: api/<UrlController>
        [HttpGet]
        public async Task<ActionResult<List<Url>>> GetAll()
        {
            try
            {
                List<Url> result = new List<Url>();
                result = await _db_context.GetAllUrl();
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

        // GET api/<UrlController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Url>> Get(int id)
        {
            try
            {
                var result = await _db_context.GetUrl(id);
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

        // POST api/<UrlController>
        [HttpPost]
        public async Task<ActionResult<int>> Post(string original_url, int creator_id)
        {
            bool condition = true;
            int url_id;
            string short_code = "";
            try
            {
                var check_user = _db_context.GetUser(creator_id);
                if(check_user == null)
                {
                    return NotFound($"User with ID: {creator_id} not exist");
                }

                while (condition)
                {
                    short_code = GenerateShort.GenerateShortUrl();
                    Url is_exist = await _db_context.GetUrlByShort(short_code);
                    if (is_exist == null)
                    {
                        condition = false;
                    }
                }

                Guid token = AccessTokenUrl.GenerateAccessToken();
                url_id = await _db_context.CreateUrl(original_url, short_code, token, creator_id);
                return Ok(url_id);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e}");
            }
        }

        // PUT api/<UrlController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UrlController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
