using Entities.Models;

namespace Contracts
{
    public interface IEmployeeRepository
    {
        public void EmployeeMethod();

        IEnumerable<Employee> GetEmployees(Guid companyId, bool trackChanges);

        Employee GetEmployee(Guid companyId, Guid id, bool trackChanges);
    }
}
