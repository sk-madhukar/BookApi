using BookApi.Models;
using BookApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BookApi.Controllers
{
    [Route("books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;
        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        #region [GetAll - Fetching all books at available]
        /*** 
         ActionResult<T> is a flexible return type that:
         Can return a value of type T (e.g., Book, IEnumerable<Book>)
         Or return an HTTP response like NotFound(), BadRequest(), etc.

         This makes it ideal for APIs where you might return either:

         Data (e.g., a list of books)
         Or an error/status (e.g., 404 Not Found)
         ***/
        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetAll() => Ok(_bookService.GetAll());
        #endregion

        #region [Fetch by Id]
        [HttpGet("{id}")]
        public ActionResult<Book> GetById(int id)
        {
            var book = _bookService.GetById(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }
        #endregion

        #region [Add a new book]
        /***
        1. [HttpPost] This attribute tells ASP.NET Core that this method should handle HTTP POST requests. It's typically used to create new resources.
        2. public ActionResult<Book> AddBook(Book book)

            This method takes a Book object from the request body.
            It returns an ActionResult<Book>, which allows you to return either:
                1. The created book object
                2. Or an HTTP status code (like 201 Created, 400 Bad Request, etc.)
        3. _bookService.Add(book);
            This line adds the book to your data store (e.g., database or in-memory list). The actual implementation of _bookService handles the persistence logic.
        4. return CreatedAtAction(...):
            1. CreatedAtAction is a helper method that returns:
                HTTP 201 Created status
                A Location header pointing to the newly created resource
                The created object in the response body
            2. nameof(GetById) refers to the method that can retrieve the book by its ID (usually a GET endpoint like GetById(int id)).
            3. new { id = book.Id } builds the route values for the GetById method.
            4. book is the actual object returned in the response.
         ***/

        [HttpPost]
        public ActionResult<Book> AddBook(Book book)
        {
            _bookService.Add(book);
            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }
        #endregion

        #region [Update a book based on Id]
        /***
        1. [HttpPut("{id}")]
            a. This attribute maps the method to an HTTP PUT request.
            b. The {id} in the route means the client must send the ID of the book in the URL, like: PUT /api/books/101
                 What is a PUT Request? 
                 PUT is a standard HTTP method used to update an existing resource.
                 It typically replaces the entire resource with the new data provided in the request body.   
        2. public ActionResult<Book> Update(int Id, Book book)
            a. This method takes:
                i. Id: from the URL path.
                ii. book: from the request body (usually JSON).
            b. It returns an ActionResult<Book>, allowing you to return either:
                i. A success response (like NoContent())
                ii. Or an error (like NotFound())
        3. var existing = _bookService.GetById(Id);: Checks if a book with the given ID exists in the system.
        4. if(existing is null): If no book is found, return a 404 Not Found response.
        5. _bookService.Update(Id, book);: Updates the existing book with the new data.
        6. return NoContent();
            a. Returns a 204 No Content response, which means:
                i. The update was successful.
                ii. No data is returned in the response body.
         ***/
        [HttpPut("{id}")]
        public ActionResult<Book> Update(int Id, Book book)
        {
            var existing = _bookService.GetById(Id);
            if (existing is null)
            {
                return NotFound();
            }
            _bookService.Update(Id, book);
            return NoContent();
        }
        /***
         Why Use NoContent()?
         It's a standard RESTful response for successful PUT operations.
         It tells the client: "Update succeeded, but there's nothing new to show.
         ***/
        #endregion

        #region [Deleting book based on Id]
        /***
         1. [HttpDelete("{id}")]
            a. This attribute maps the method to an HTTP DELETE request.
            b. The {id} in the route means the client must send the ID of the book in the URL, like: DELETE /api/books/101
         2. public ActionResult Delete(int Id)
            a. This method takes the Id of the book to be deleted.
            b. It returns an ActionResult, which allows you to return:
                i. A success response (NoContent())
                ii. Or an error (NotFound())
        3. var exist = _bookService.GetById(Id); - Checks if a book with the given ID exists in the system.
        4. if(exist is null) - If no book is found, return a 404 Not Found response.
        5. _bookService.Delete(Id); - Deletes the book from the data store.
        6. return NoContent(); - Returns a 204 No Content response, which means:

            a. The deletion was successful.
            b. No data is returned in the response body.
        
        Why Use NoContent()?
        It's a standard RESTful response for successful DELETE operations.
        It tells the client: "Deletion succeeded, and there's nothing more to send back."
        ***/
        [HttpDelete("{id}")]
        public ActionResult Delete(int Id) 
        { 
            var exist = _bookService.GetById(Id);
            if(exist is null)
            {
                return NotFound();
            }

            _bookService.Delete(Id);
            return NoContent();
        }
        #endregion
    }
}
