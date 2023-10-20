using Contracts;
using Entities;
using Entities.Models;

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

        public Book GetBook(Guid authorId, Guid id, bool trackChanges)
        {
            return FindByCondition(
                e => e.AuthorId.Equals(authorId) && e.Id.Equals(id), trackChanges)
                .SingleOrDefault();
        }

        public IEnumerable<Book> GetBooks(Guid authorId, bool trackChanges)
        {
            return FindByCondition(e => e.AuthorId.Equals(authorId), trackChanges).OrderBy(e => e.Name);
        }
    }
}
