using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class AuthorRepository : RepositoryBase<Author>, IAuthorRepository
    {
        public AuthorRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void AuthorMethod()
        {
            
        }
    }
}
