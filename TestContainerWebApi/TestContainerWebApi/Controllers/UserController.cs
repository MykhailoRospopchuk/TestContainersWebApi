using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TestContainerWebApi.db;
using TestContainerWebApi.Models.ModelDto;

namespace TestContainerWebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "manager")]
    public class UserController : ControllerBase
    {
        private readonly AdoDbContext _dbContext;

        public UserController(AdoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<UserController>
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAll()
        {
            try
            {
                List<UserDto> result = new();
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
        public async Task<ActionResult<UserDto>> Get(int id)
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

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<int>> Put(int id)
        {
            try
            {
                UserDto user;
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
