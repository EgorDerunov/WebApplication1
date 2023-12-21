using Contracts;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.ActionFilter
{
    public class ValidateBookForAuthorExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;

        public ValidateBookForAuthorExistsAttribute(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = (method.Equals("PUT") || method.Equals("PATCH")) ? true : false;
            var authorId = (Guid)context.ActionArguments["authorId"];
            var author = await _repository.Author.GetAuthorAsync(authorId, false);

            if (author == null)
            {
                _logger.LogInfo($"Author with id: {authorId} doesn't exist in the database.");
                context.Result = new NotFoundResult();
                return;
            }

            var id = (Guid)context.ActionArguments["id"];
            var book = await _repository.Book.GetBookAsync(authorId, id, trackChanges);

            if (book == null)
            {
                _logger.LogInfo($"Book with id: {id} doesn't exist in the database.");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("book", book);
                await next();
            }
        }
    }
}
