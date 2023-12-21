using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

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

        public void CreateAuthor(Author author)
        {
            Create(author);
        }

        public void DeleteAuthor(Author author)
        {
            Delete(author);
        }

        public async Task<IEnumerable<Author>> GetAllAuthorsAsync(bool trackChanges)
        {
            return await FindAll(trackChanges).OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<Author> GetAuthorAsync(Guid authorId, bool trackChanges)
        { 
            return await FindByCondition(c => c.Id.Equals(authorId), trackChanges).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Author>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            return await FindByCondition(x =>
                ids.Contains(x.Id), trackChanges).ToListAsync();
        }
    }
}
