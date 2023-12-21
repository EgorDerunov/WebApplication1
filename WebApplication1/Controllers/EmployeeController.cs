using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.Employee;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/companies/{companyId}/employees")]
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
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId)
        {
            var company = await repositoryManager.Company.GetCompanyAsync(companyId, trackChanges: false);

            if (company == null)
            {
                loggerManager.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }

            var employeesFromDb = repositoryManager.Employee.GetEmployeesAsync(companyId, trackChanges: false);
            var employeesDto = mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);
            return Ok(employeesDto);
        }

        [HttpGet("{id}", Name = "GetEmployeeForCompany")]
        public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid id)
        {
            var company = await repositoryManager.Company.GetCompanyAsync(companyId, trackChanges: false);

            if (company == null)
            {
                loggerManager.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }

            var employeeDb = repositoryManager.Employee.GetEmployeeAsync(companyId, id, trackChanges: false);

            if (employeeDb == null)
            {
                loggerManager.LogInfo($"Employee with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            var employee = mapper.Map<EmployeeDto>(employeeDb);

            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee)
        {
            if (employee == null)
            {
                loggerManager.LogError("EmployeeForCreationDto object sent from client is null.");
                return BadRequest("EmployeeForCreationDto object is null");
            }

            if (!ModelState.IsValid)
            {
                loggerManager.LogError("Invalid model state for the EmployeeForCreationDto object");
                return UnprocessableEntity(ModelState);
            }

            var company = await repositoryManager.Company.GetCompanyAsync(companyId, trackChanges: false);

            if (company == null)
            {
                loggerManager.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }

            var employeeEntity = mapper.Map<Employee>(employee);
            repositoryManager.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
            await repositoryManager.SaveAsync();

            var employeeToReturn = mapper.Map<EmployeeDto>(employeeEntity);

            return CreatedAtRoute("GetEmployeeForCompany", new
            {
                companyId,
                id = employeeToReturn.Id
            }, employeeToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmloyeeForCompany(Guid companyId, Guid id)
        {
            var company = await repositoryManager.Company.GetCompanyAsync(companyId, trackChanges: false);

            if (company == null)
            {
                loggerManager.LogInfo("$\"Company with id: {companyId} doesn't exist in the\r\ndatabase.\"");
                return NotFound();
            }

            var employeeForComapny = await repositoryManager.Employee.GetEmployeeAsync(companyId, id, trackChanges: false);

            if (employeeForComapny == null)
            {
                loggerManager.LogInfo($"Employee with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            repositoryManager.Employee.DeleteEmployee(employeeForComapny);
            await repositoryManager.SaveAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee)
        {
            if (employee == null)
            {
                loggerManager.LogError("EmployeeForUpdateDto object sent from client is null.");
                return BadRequest("EmployeeForUpdateDto object is null");
            }

            if (!ModelState.IsValid)
            {
                loggerManager.LogError("Invalid model state for the EmployeeForUpdateDto object");
                return UnprocessableEntity(ModelState);
            }

            var company = await repositoryManager.Company.GetCompanyAsync(companyId, trackChanges: false);

            if (company == null)
            {
                loggerManager.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }

            var employeeEntity = await repositoryManager.Employee.GetEmployeeAsync(companyId, id, trackChanges: true);

            if (employeeEntity == null)
            {
                loggerManager.LogInfo($"Employee with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            mapper.Map(employee, employeeEntity);
            await repositoryManager.SaveAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(
            Guid companyId,
            Guid id,
            [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {

            if (patchDoc == null)
            {
                loggerManager.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }

            var company = await repositoryManager.Company.GetCompanyAsync(companyId, trackChanges: false);

            if (company == null)
            {
                loggerManager.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }

            var employeeEntity = await repositoryManager.Employee.GetEmployeeAsync(companyId, id, trackChanges: true);

            if (employeeEntity == null)
            {
                loggerManager.LogInfo($"Employee with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            var employeeToPatch = mapper.Map<EmployeeForUpdateDto>(employeeEntity);

            patchDoc.ApplyTo(employeeToPatch);
            TryValidateModel(employeeToPatch);

            if (!ModelState.IsValid)
            {
                loggerManager.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }

            mapper.Map(employeeToPatch, employeeEntity);
            await repositoryManager.SaveAsync();

            return NoContent();
        }
    }
}
