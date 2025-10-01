using BookApi.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;

namespace BookApi.Controllers
{
    [ApiController]
    [Route("books-dapper")]
    public class BooksDapperController : ControllerBase
    {
        private readonly SqlConnection _sqlConnection;
        public BooksDapperController(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        [HttpGet]
        public async Task<IEnumerable<Book>> GetAll()
        {
            var sql = "SELECT * FROM Books";
            return await _sqlConnection.QueryAsync<Book>(sql);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetById(int id)
        {
            var sql = "SELECT * FROM Book WHERE Id = @id";
            var book = await _sqlConnection.QueryFirstOrDefaultAsync<Book>(sql, new {Id = id});
            return book is not null ? Ok(book) : NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Book book)
        {
            var sql = "INSERT INTO Books (Title, Author, Year) VALUES (@Title, @Author, @Year); SELECT CAST(SCOPE_IDENTITY() as int);";
            var id = await _sqlConnection.ExecuteScalarAsync<int>(sql, book);
            book.Id = id;
            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Book book)
        {
            var sql = "UPDATE Books SET Title = @Title, Author = @Author, Year = @Year WHERE Id = @Id";
            var rowsAffected = await _sqlConnection.ExecuteAsync(sql, new
            {
                book.Title,
                book.Author,
                book.Year,
                Id = id
            });
            return rowsAffected > 0 ? NoContent() : NotFound();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var sql = "DELETE FROM Books WHERE Id = @Id";
            var rowsAffected = await _sqlConnection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0 ? NoContent() : NotFound();
        }
        
        
    }
}
