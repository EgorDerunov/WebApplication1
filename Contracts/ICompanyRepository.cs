using Entities.Models;

namespace Contracts
{
    public interface ICompanyRepository
    {
        public void CompanyMethod();

        IEnumerable<Company> GetAllCompanies(bool trackChanges);

        Company GetCompany(Guid companyId, bool trackChanges);
    }
}
