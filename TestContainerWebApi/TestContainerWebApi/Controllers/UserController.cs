using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TestContainerWebApi.db;
using TestContainerWebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestContainerWebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AdoDbContext _db_context;

        public UserController(AdoDbContext db_context)
        {
            _db_context = db_context;
        }

        // GET: api/<UserController>
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            try
            {
                List<User> result = new();
                result = await _db_context.GetAllUsers();
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
                var result = await _db_context.GetUser(id);
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
                user_id = await _db_context.CreateUser();
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
                user = await _db_context.UpdateUser(id);
                return Ok(user);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e}");
            }
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
