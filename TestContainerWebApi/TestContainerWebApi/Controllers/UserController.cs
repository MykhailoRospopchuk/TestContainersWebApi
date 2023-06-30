using Microsoft.AspNetCore.Mvc;
using TestContainerWebApi.db;
using TestContainerWebApi.Models;

namespace TestContainerWebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AdoDbContext _dbContext;

        public UserController(AdoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<UserController>
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            try
            {
                List<User> result = new();
                result = await _dbContext.GetAllUsers();
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

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            try
            {
                var result = await _dbContext.GetUser(id);
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

        // POST api/<UserController>
        [HttpPost]
        public async Task<ActionResult<int>> Post()
        {
            try
            {
                int user_id;
                user_id = await _dbContext.CreateUser();
                return Ok(user_id);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e}");
            }
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<int>> Put(int id)
        {
            try
            {
                User user;
                user = await _dbContext.UpdateUser(id);
                return Ok(user);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e}");
            }
        }
    }
}
