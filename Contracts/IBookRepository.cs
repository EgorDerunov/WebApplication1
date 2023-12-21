using Entities.Models;

namespace Contracts
{
    public interface IBookRepository
    {
        public void BookMethod();

        Task<IEnumerable<Book>> GetBooksAsync(Guid authorId, bool trackChanges);

        Task<Book> GetBookAsync(Guid authorId, Guid id, bool trackChanges);

        void CreateBookForAuthor(Guid authorId, Book book);

        void DeleteBook(Book book);
    }
}
