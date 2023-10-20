using Entities.Models;

namespace Contracts
{
    public interface IAuthorRepository
    {
        public void AuthorMethod();

        IEnumerable<Author> GetAllAuthors(bool trackChanges);

        Author GetAuthor(Guid authorId, bool trackChanges);
    }
}
