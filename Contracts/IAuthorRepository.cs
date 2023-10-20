using Entities.Models;

namespace Contracts
{
    public interface IAuthorRepository
    {
        public void AuthorMethod();

        IEnumerable<Author> GetAllAuthors(bool trackChanges);
    }
}
