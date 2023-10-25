using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.ModelBinders;

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

        [HttpGet("{id}", Name = "AuthorById")]
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

        [HttpGet("collection/{ids}", Name = "AuthorCollection")]
        public IActionResult GetAuthorCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }

            var authorEntityes = _repository.Author.GetByIds(ids, trackChanges: false);

            if (ids.Count() != authorEntityes.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }

            var companiesToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authorEntityes);

            return Ok(companiesToReturn);
        }

        [HttpPost]
        public IActionResult CreateAuthor([FromBody] AuthorForCreationDto author)
        {
            if (author == null)
            {
                _logger.LogError("AuthorForCreationDto object sent from client is null.");
                return BadRequest("AuthorForCreationDto object is null");
            }

            var authorEntry = _mapper.Map<Author>(author);
            _repository.Author.CreateAuthor(authorEntry);
            _repository.Save();

            var authorToReturn = _mapper.Map<AuthorDto>(authorEntry);

            return CreatedAtRoute("AuthorById", new { id = authorToReturn.Id }, authorToReturn);
        }

        [HttpPost("collection")]
        public IActionResult CreateAuthorCollection([FromBody] IEnumerable<AuthorForCreationDto> authorCollection)
        {
            if (authorCollection == null)
            {
                _logger.LogError("Author collection sent from client is null.");
                return BadRequest("Author collection is null");
            }

            var authorEntities = _mapper.Map<IEnumerable<Author>>(authorCollection);

            foreach (var author in authorEntities)
            {
                _repository.Author.CreateAuthor(author);
            }

            _repository.Save();

            var authorCollectionToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authorEntities);
            var ids = string.Join(",", authorCollectionToReturn.Select(c => c.Id));

            return CreatedAtRoute("AuthorCollection", new { ids }, authorCollectionToReturn);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAuthor(Guid id)
        {
            var author = _repository.Author.GetAuthor(id, trackChanges: false);

            if (author == null)
            {
                _logger.LogInfo($"Author with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            _repository.Author.DeleteAuthor(author);
            _repository.Save();
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAuthor(Guid id, [FromBody] AuthorForUpdateDto author)
        {
            if (author == null)
            {
                _logger.LogError("AuthorForUpdateDto object sent from client is null.");
                return BadRequest("AuthorForUpdateDto object is null");
            }

            var authorEntity = _repository.Author.GetAuthor(id, trackChanges: true);

            if (authorEntity == null)
            {
                _logger.LogInfo($"Author with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            _mapper.Map(author, authorEntity);
            _repository.Save();

            return NoContent();
        }
    }
}
