using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetBooksForAuthor(Guid authorId)
        {
            var author = repositoryManager.Author.GetAuthor(authorId, trackChanges: false);

            if (author == null)
            {
                loggerManager.LogInfo($"Author with id: {authorId} doesn't exist in the database.");
                return NotFound();
            }

            var booksFromDb = repositoryManager.Book.GetBooks(authorId, trackChanges: false);
            var booksDto = mapper.Map<IEnumerable<BookDto>>(booksFromDb);
            return Ok(booksDto);
        }

        [HttpGet("{id}", Name = "GeBookForAuthor")]
        public IActionResult GetBookForAuthor(Guid authorId, Guid id)
        {
            var author = repositoryManager.Author.GetAuthor(authorId, trackChanges: false);

            if (author == null)
            {
                loggerManager.LogInfo($"Author with id: {authorId} doesn't exist in the database.");
                return NotFound();
            }

            var bookDb = repositoryManager.Book.GetBook(authorId, id, trackChanges: false);

            if (bookDb == null)
            {
                loggerManager.LogInfo($"Book with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            var employee = mapper.Map<BookDto>(bookDb);

            return Ok(employee);
        }

        [HttpPost]
        public IActionResult CreateBookForAuthor(Guid authorId, [FromBody] BookForCreationDto book)
        {
            if (book == null)
            {
                loggerManager.LogError("BookForCreationDto object sent from client is null.");
                return BadRequest("BookForCreationDto object is null");
            }

            var autohr = repositoryManager.Author.GetAuthor(authorId, trackChanges: false);

            if (autohr == null)
            {
                loggerManager.LogInfo($"Author with id: {authorId} doesn't exist in the database.");
                return NotFound();
            }

            var bookEntity = mapper.Map<Book>(book);
            repositoryManager.Book.CreateBookForAuthor(authorId, bookEntity);
            repositoryManager.Save();

            var bookToReturn = mapper.Map<BookDto>(bookEntity);

            return CreatedAtRoute("GetBookForAuthor", new
            {
                authorId,
                id = bookToReturn.Id
            }, bookToReturn);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBookForAuthor(Guid authorId, Guid id)
        {
            var author = repositoryManager.Author.GetAuthor(authorId, trackChanges: false);

            if (author == null)
            {
                loggerManager.LogInfo("$\"Author with id: {authorId} doesn't exist in the\r\ndatabase.\"");
                return NotFound();
            }

            var bookForAuthor = repositoryManager.Book.GetBook(authorId, id, trackChanges: false);

            if (bookForAuthor == null)
            {
                loggerManager.LogInfo($"Book with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            repositoryManager.Book.DeleteBook(bookForAuthor);
            repositoryManager.Save();
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBookForAuthor(Guid authorId, Guid id, [FromBody] BookForUpdateDto book)
        {
            if (book == null)
            {
                loggerManager.LogError("BookForUpdateDto object sent from client is null.");
                return BadRequest("BookForUpdateDto object is null");
            }

            var author = repositoryManager.Author.GetAuthor(authorId, trackChanges: false);

            if (author == null)
            {
                loggerManager.LogInfo($"Author with id: {authorId} doesn't exist in the database.");
                return NotFound();
            }

            var bookEntity = repositoryManager.Book.GetBook(authorId, id, trackChanges: true);

            if (bookEntity == null)
            {
                loggerManager.LogInfo($"Book with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            mapper.Map(book, bookEntity);
            repositoryManager.Save();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateBookForAuthor(
            Guid authorId,
            Guid id,
            [FromBody] JsonPatchDocument<BookForUpdateDto> patchDoc)
        {

            if (patchDoc == null)
            {
                loggerManager.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }

            var author = repositoryManager.Author.GetAuthor(authorId, trackChanges: false);

            if (author == null)
            {
                loggerManager.LogInfo($"Author with id: {authorId} doesn't exist in the database.");
                return NotFound();
            }

            var bookEntity = repositoryManager.Book.GetBook(authorId, id, trackChanges: true);

            if (bookEntity == null)
            {
                loggerManager.LogInfo($"Book with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            var bookToPatch = mapper.Map<BookForUpdateDto>(bookEntity);

            patchDoc.ApplyTo(bookToPatch);
            mapper.Map(bookToPatch, bookEntity);

            repositoryManager.Save();

            return NoContent();
        }
    }
}
