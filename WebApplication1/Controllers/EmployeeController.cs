using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly ILoggerManager loggerManager;
        private readonly IMapper mapper;

        public EmployeeController(IRepositoryManager _repositoryManager, ILoggerManager _loggerManager, IMapper _mapper)
        {
            repositoryManager = _repositoryManager;
            loggerManager = _loggerManager;
            mapper = _mapper;
        }

        [HttpGet]
        public IActionResult GetEmployeesForCompany(Guid companyId)
        {
            var company = repositoryManager.Company.GetCompany(companyId, trackChanges: false);

            if (company == null)
            {
                loggerManager.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }

            var employeesFromDb = repositoryManager.Employee.GetEmployees(companyId, trackChanges: false);
            var employeesDto = mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);
            return Ok(employeesDto);
        }

        [HttpGet("{id}")]
        public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)
        {
            var company = repositoryManager.Company.GetCompany(companyId, trackChanges: false);

            if (company == null)
            {
                loggerManager.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }

            var employeeDb = repositoryManager.Employee.GetEmployee(companyId, id, trackChanges: false);

            if (employeeDb == null)
            {
                loggerManager.LogInfo($"Employee with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            var employee = mapper.Map<EmployeeDto>(employeeDb);

            return Ok(employee);
        }
    }
}
