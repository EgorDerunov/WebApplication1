using Entities.Models;
using Entities.RequestFeatures;

namespace Contracts
{
    public interface IBookRepository
    {
        public void BookMethod();

        Task<PagedList<Book>> GetBooksAsync(Guid authorId, BookParameters employeeParametrs, bool trackChanges);

        Task<Book> GetBookAsync(Guid authorId, Guid id, bool trackChanges);

        void CreateBookForAuthor(Guid authorId, Book book);

        void DeleteBook(Book book);
    }
}
