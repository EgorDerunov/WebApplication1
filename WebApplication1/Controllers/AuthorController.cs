﻿using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.Author;
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
        public async Task<IActionResult> GetAuthors()
        {
            var authors = await _repository.Author.GetAllAuthorsAsync(trackChanges: false);
            var authorsDto = _mapper.Map<IEnumerable<AuthorDto>>(authors);
            return Ok(authorsDto);
        }

        [HttpGet("{id}", Name = "AuthorById")]
        public async Task<IActionResult> GetAuthor(Guid id)
        {
            var author = await _repository.Author.GetAuthorAsync(id, trackChanges: false);
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
        public async Task<IActionResult> GetAuthorCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }

            var authorEntityes = await _repository.Author.GetByIdsAsync(ids, trackChanges: false);

            if (ids.Count() != authorEntityes.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }

            var companiesToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authorEntityes);

            return Ok(companiesToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorForCreationDto author)
        {
            if (author == null)
            {
                _logger.LogError("AuthorForCreationDto object sent from client is null.");
                return BadRequest("AuthorForCreationDto object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the AuthorForCreationDto object");
                return UnprocessableEntity(ModelState);
            }

            var authorEntry = _mapper.Map<Author>(author);
            _repository.Author.CreateAuthor(authorEntry);
            await _repository.SaveAsync();

            var authorToReturn = _mapper.Map<AuthorDto>(authorEntry);

            return CreatedAtRoute("AuthorById", new { id = authorToReturn.Id }, authorToReturn);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateAuthorCollection([FromBody] IEnumerable<AuthorForCreationDto> authorCollection)
        {
            if (authorCollection == null)
            {
                _logger.LogError("Author collection sent from client is null.");
                return BadRequest("Author collection is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the author collection object");
                return UnprocessableEntity(ModelState);
            }

            var authorEntities = _mapper.Map<IEnumerable<Author>>(authorCollection);

            foreach (var author in authorEntities)
            {
                _repository.Author.CreateAuthor(author);
            }

            await _repository.SaveAsync();

            var authorCollectionToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authorEntities);
            var ids = string.Join(",", authorCollectionToReturn.Select(c => c.Id));

            return CreatedAtRoute("AuthorCollection", new { ids }, authorCollectionToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(Guid id)
        {
            var author = await _repository.Author.GetAuthorAsync(id, trackChanges: false);

            if (author == null)
            {
                _logger.LogInfo($"Author with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            _repository.Author.DeleteAuthor(author);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(Guid id, [FromBody] AuthorForUpdateDto author)
        {
            if (author == null)
            {
                _logger.LogError("AuthorForUpdateDto object sent from client is null.");
                return BadRequest("AuthorForUpdateDto object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the AuthorForUpdateDto object");
                return UnprocessableEntity(ModelState);
            }

            var authorEntity = await _repository.Author.GetAuthorAsync(id, trackChanges: true);

            if (authorEntity == null)
            {
                _logger.LogInfo($"Author with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            _mapper.Map(author, authorEntity);
            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
