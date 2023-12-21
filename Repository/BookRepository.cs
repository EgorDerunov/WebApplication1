using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace Repository
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void BookMethod()
        {
            
        }

        public void CreateBookForAuthor(Guid authorId, Book book)
        {
            book.AuthorId = authorId;
            Create(book);
        }

        public void DeleteBook(Book book)
        {
            Delete(book);
        }

        public async Task<Book> GetBookAsync(Guid authorId, Guid id, bool trackChanges)
        {
            return await FindByCondition(
                e => e.AuthorId.Equals(authorId) && e.Id.Equals(id), trackChanges)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksAsync(Guid authorId, bool trackChanges)
        {
            return await FindByCondition(e => e.AuthorId.Equals(authorId), trackChanges).OrderBy(e => e.Name).ToListAsync();
        }
    }
}
