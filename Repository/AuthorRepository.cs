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

        public void CreateAuthor(Author author)
        {
            Create(author);
        }

        public IEnumerable<Author> GetAllAuthors(bool trackChanges)
        {
            return FindAll(trackChanges).OrderBy(c => c.Name).ToList();
        }

        public Author GetAuthor(Guid authorId, bool trackChanges)
        {
            return FindByCondition(c => c.Id.Equals(authorId), trackChanges).SingleOrDefault();
        }

        public IEnumerable<Author> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
        {
            return FindByCondition(x =>
                ids.Contains(x.Id), trackChanges).ToList();
        }
    }
}
