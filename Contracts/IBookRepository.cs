using Entities.Models;

namespace Contracts
{
    public interface IBookRepository
    {
        public void BookMethod();

        IEnumerable<Book> GetBooks(Guid authorId, bool trackChanges);

        Book GetBook(Guid authorId, Guid id, bool trackChanges);
    }
}
