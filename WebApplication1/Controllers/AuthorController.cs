using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public AuthorController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAuthors()
        {
            var authors = _repository.Author.GetAllAuthors(trackChanges: false);
            var authorsDto = _mapper.Map<IEnumerable<AuthorDto>>(authors);
            return Ok(authorsDto);
        }

        [HttpGet("{id}")]
        public IActionResult GetAuthor(Guid id)
        {
            var author = _repository.Author.GetAuthor(id, trackChanges: false);
            if (author == null)
            {
                _logger.LogInfo($"Author with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            else
            {
                var authorDto = _mapper.Map<AuthorDto>(author);
                return Ok(authorDto);
            }
        }
    }
}
