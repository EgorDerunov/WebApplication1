namespace Contracts
{
    public interface IRepositoryManager
    {
        ICompanyRepository Company { get; }
        IEmployeeRepository Employee { get; }
        IAuthorRepository Author { get; }
        IBookRepository Book { get; }
        void Save();
    }
}
