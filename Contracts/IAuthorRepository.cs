using Entities.Models;

namespace Contracts
{
    public interface IAuthorRepository
    {
        public void AuthorMethod();

        IEnumerable<Author> GetAllAuthors(bool trackChanges);

        Author GetAuthor(Guid authorId, bool trackChanges);

        void CreateAuthor(Author author);

        IEnumerable<Author> GetByIds(IEnumerable<Guid> ids, bool trackChanges);
    }
}
