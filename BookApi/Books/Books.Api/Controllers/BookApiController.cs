/*
 * Books Microservice
 *
 * A service to store books and request data about them.
 *
 * The version of the OpenAPI document: 0.0.1
 * 
 * Generated by: https://openapi-generator.tech
 */

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Org.OpenAPITools.Models;
using Books.Api.Services;
using AutoMapper;
using Books.Api.Models;
using Books.Api.Entity;

namespace Org.OpenAPITools.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class BookApiController : ControllerBase
    { 
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BookApiController(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository  ?? throw new ArgumentNullException(nameof(bookRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Deletes a book from the database</remarks>
        /// <param name="id">The id of the book</param>
        /// <response code="204">The book with the given id is deleted</response>
        /// <response code="404">The Book with the given id is not found</response>
        [HttpDelete]
        [Route("/book/{Id}")]
        [SwaggerOperation("DeleteBookById")]
        public async Task<ActionResult> DeleteBookById([FromRoute (Name = "Id")][Required]int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _bookRepository.DeleteBookAsync(book);
            await _bookRepository.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Gives back a stored book by it's id</remarks>
        /// <param name="id">The id of the book</param>
        /// <response code="200">A sucsessful response that gives back a book by id</response>
        /// <response code="404">The Book with the given id is not found</response>
        [HttpGet("{id}", Name = "GetBook")]
        [SwaggerOperation("GetBookById")]
        [SwaggerResponse(statusCode: 200, type: typeof(Book), description: "A sucsessful response that gives back a book by id")]
        public async Task<IActionResult> GetBookById([FromRoute][Required]int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            if(book == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<Book>(book));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Gives back a list of stored books</remarks>
        /// <response code="200">A sucsessful response that lists the stored books</response>
        [HttpGet]
        [Route("/books")]
        [SwaggerOperation("GetBooks")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<Book>), description: "A sucsessful response that lists the stored books")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            var bookEntities = await _bookRepository.GetBooksAsync();
            return Ok(bookEntities);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Updates a book by it's id</remarks>
        /// <param name="id">The id of the book</param>
        /// <param name="title">The title of the book</param>
        /// <response code="200">The book with the given id is updated</response>
        /// <response code="404">The Book with the given id is not found</response>
        [HttpPut]
        [Route("/book/{Id}")]
        [Consumes("application/json")]
        [SwaggerOperation("PutBookById")]
        public async Task<ActionResult> PutBookById([FromRoute (Name = "Id")][Required]int id,
            BookForUpdate book)
        {
            var bookEntity = await _bookRepository.GetBookByIdAsync(id);
            if (bookEntity == null)
            {
                return NotFound();
            }
            
            _mapper.Map(book, bookEntity);
            await _bookRepository.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Adds a book to the database</remarks>
        /// <param name="title">The title of the book</param>
        /// <response code="201">The book with the given title is stored in the database</response>
        [HttpPost]
        [Route("/book")]
        [Consumes("application/json")]
        [SwaggerOperation("PostBook")]
        public async Task<ActionResult<Book>> PostBook(BookForCreation book)
        {
            var finalBook = _mapper.Map<BookEntity>(book);
            await _bookRepository.AddBookAsync(finalBook);
            await _bookRepository.SaveChangesAsync();

            return CreatedAtRoute("GetBook", new { id = finalBook.Id }, _mapper.Map<Book>(finalBook));
        }
    }
}
