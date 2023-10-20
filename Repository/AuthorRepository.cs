using Contracts;
using Entities;
using Entities.Models;
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

        public IEnumerable<Author> GetAllAuthors(bool trackChanges)
        {
            return FindAll(trackChanges).OrderBy(c => c.Name).ToList();
        }

        public Author GetAuthor(Guid authorId, bool trackChanges)
        {
            return FindByCondition(c => c.Id.Equals(authorId), trackChanges).SingleOrDefault();
        }
}
