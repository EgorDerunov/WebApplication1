using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorV2Controller : ControllerBase
    {
        private readonly IRepositoryManager _repository;

        public AuthorV2Controller(IRepositoryManager repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAuthors()
        {
            var authors = await _repository.Author.GetAllAuthorsAsync(trackChanges: false);
            return Ok(authors);
        }
    }
}
