using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataLibrary.Models;
using DataLibrary.Context;
using System.Data.SqlClient;
using Dapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ShemosToDoAng.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private IConfiguration _config;
        public ToDoController(IConfiguration config)
        {
            _config = config;
        }
        [HttpGet]
        public async Task<ActionResult<List<ToDoModel>>> GetToDo()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var tasks = await connection.QueryAsync<ToDoModel>("SELECT * FROM ToDo");
            return Ok(tasks);
        }

        [HttpPost(nameof(InsertToDo))]
        public async Task<ActionResult> InsertToDo([FromBody]ToDoModel newToDo)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            var query = "INSERT INTO ToDo (Title, Description, DateTime) VALUES (@Title, @Description, @Date)";

            var parameters = new
            {
                Title = newToDo.Title,
                Description = newToDo.Description,
                Date = DateTime.Now
            };

            var rowsAffected = await connection.ExecuteAsync(query, parameters);

            if (rowsAffected > 0)
            {
                return CreatedAtAction(nameof(GetToDo), new { Id = newToDo.Id }, newToDo);
            }
            else
            {
                return BadRequest("Insert failed.");
            }
        }

        [HttpPut(nameof(PutToDo))]
        public async Task<ActionResult> PutToDo([FromBody] ToDoModel newToDo)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var query = "UPDATE ToDo set Title=@Title, Description=@Description Where Id=@Id";
            var parameters = new
            {
                Id = newToDo.Id,
                Title = newToDo.Title,
                Description = newToDo.Description,
                Date = DateTime.Now
            };
            var rowsAffected = await connection.ExecuteAsync(query, parameters);
            return CreatedAtAction(nameof(GetToDo), new { Id = newToDo.Id }, newToDo);
        }

        [HttpDelete(nameof(DeleteToDo))]
        public async Task<int> DeleteToDo([FromQuery] int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var query = "Delete from ToDo Where Id=@Id";
            var parameters = new
            {
                Id = id
            };
            var rowsAffected = await connection.ExecuteAsync(query, parameters);
            return id;
        }

        [HttpGet(nameof(GetIDToDo))]
        public async Task<ActionResult<List<ToDoModel>>> GetIDToDo(int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var query = "select * from ToDo where id=@id";
            var parameters = new
            {
                id = id
            };
            var rowsAffected = await connection.ExecuteAsync(query, parameters);
            return Ok(rowsAffected);
        }
    }
}
