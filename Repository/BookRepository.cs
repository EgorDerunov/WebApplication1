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
    }
}
