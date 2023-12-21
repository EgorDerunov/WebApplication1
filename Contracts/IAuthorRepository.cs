using Entities.Models;

namespace Contracts
{
    public interface IAuthorRepository
    {
        public void AuthorMethod();

        Task<IEnumerable<Author>> GetAllAuthorsAsync(bool trackChanges);

        Task<Author> GetAuthorAsync(Guid authorId, bool trackChanges);

        void CreateAuthor(Author author);

        Task<IEnumerable<Author>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);

        void DeleteAuthor(Author author);

    }
}
