using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.Book;
using Entities.DataTransferObjects.Employee;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.Design;
using WebApplication1.ActionFilter;

namespace WebApplication1.Controllers
{
    [Route("api/authors/{authorId}/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly ILoggerManager loggerManager;
        private readonly IMapper mapper;

        public BookController(IRepositoryManager _repositoryManager, ILoggerManager _loggerManager, IMapper _mapper)
        {
            repositoryManager = _repositoryManager;
            loggerManager = _loggerManager;
            mapper = _mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooksForAuthor(Guid authorId, [FromQuery] BookParameters bookParametrs)
        {
            if (!bookParametrs.ValidYearRange)
            {
                return BadRequest("Max year can't be less than min year.");
            }

            var author = await repositoryManager.Author.GetAuthorAsync(authorId, trackChanges: false);

            if (author == null)
            {
                loggerManager.LogInfo($"Author with id: {authorId} doesn't exist in the database.");
                return NotFound();
            }

            var booksFromDb = await repositoryManager.Book.GetBooksAsync(authorId, bookParametrs, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(booksFromDb.MetaData));

            var booksDto = mapper.Map<IEnumerable<BookDto>>(booksFromDb);
            return Ok(booksDto);
        }

        [HttpGet("{id}", Name = "GeBookForAuthor")]
        public async Task<IActionResult> GetBookForAuthor(Guid authorId, Guid id)
        {
            var author = await repositoryManager.Author.GetAuthorAsync(authorId, trackChanges: false);

            if (author == null)
            {
                loggerManager.LogInfo($"Author with id: {authorId} doesn't exist in the database.");
                return NotFound();
            }

            var bookDb = repositoryManager.Book.GetBookAsync(authorId, id, trackChanges: false);

            if (bookDb == null)
            {
                loggerManager.LogInfo($"Book with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            var employee = mapper.Map<BookDto>(bookDb);

            return Ok(employee);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateBookForAuthor(Guid authorId, [FromBody] BookForCreationDto book)
        {
            var bookEntity = mapper.Map<Book>(book);
            repositoryManager.Book.CreateBookForAuthor(authorId, bookEntity);
            await repositoryManager.SaveAsync();

            var bookToReturn = mapper.Map<BookDto>(bookEntity);

            return CreatedAtRoute("GetBookForAuthor", new
            {
                authorId,
                id = bookToReturn.Id
            }, bookToReturn);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateBookForAuthorExistsAttribute))]
        public async Task<IActionResult> DeleteBookForAuthor(Guid authorId, Guid id)
        {
            var bookForAuthor = HttpContext.Items["book"] as Book;

            repositoryManager.Book.DeleteBook(bookForAuthor);
            await repositoryManager.SaveAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateBookForAuthorExistsAttribute))]
        public async Task<IActionResult> UpdateBookForAuthor(Guid authorId, Guid id, [FromBody] BookForUpdateDto book)
        {
            var bookEntity = HttpContext.Items["book"] as Book;

            mapper.Map(book, bookEntity);
            await repositoryManager.SaveAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        [ServiceFilter(typeof(ValidateBookForAuthorExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateBookForAuthor(
            Guid authorId,
            Guid id,
            [FromBody] JsonPatchDocument<BookForUpdateDto> patchDoc)
        {

            if (patchDoc == null)
            {
                loggerManager.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }

            var bookEntity = await repositoryManager.Book.GetBookAsync(authorId, id, trackChanges: true);
            var bookToPatch = mapper.Map<BookForUpdateDto>(bookEntity);

            TryValidateModel(bookToPatch);

            if (!ModelState.IsValid)
            {
                loggerManager.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }

            mapper.Map(bookToPatch, bookEntity);

            await repositoryManager.SaveAsync();

            return NoContent();
        }
    }
}
