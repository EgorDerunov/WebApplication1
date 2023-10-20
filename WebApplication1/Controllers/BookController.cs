using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet("{id}")]
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
    }
}
