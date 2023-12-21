using Entities.Models;
using Entities.RequestFeatures;

namespace Contracts
{
    public interface IEmployeeRepository
    {
        public void EmployeeMethod();

        Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParametrs, bool trackChanges);

        Task<Employee> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges);

        void CreateEmployeeForCompany(Guid companyId, Employee employee);

        void DeleteEmployee(Employee employee);
    }
}
