using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.Employee;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication1.ActionFilter;

namespace WebApplication1.Controllers
{
    [Route("api/companies/{companyId}/employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly ILoggerManager loggerManager;
        private readonly IMapper mapper;
        private readonly IDataShaper<EmployeeDto> dataShaper;

        public EmployeeController(IRepositoryManager _repositoryManager, ILoggerManager _loggerManager, IMapper _mapper, IDataShaper<EmployeeDto> _dataShaper)
        {
            repositoryManager = _repositoryManager;
            loggerManager = _loggerManager;
            mapper = _mapper;
            dataShaper = _dataShaper;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId, [FromQuery] EmployeeParameters employeeParametrs)
        {
            if (!employeeParametrs.ValidAgeRange)
            {
                return BadRequest("Max age can't be less than min age.");
            }

            var company = await repositoryManager.Company.GetCompanyAsync(companyId, trackChanges: false);

            if (company == null)
            {
                loggerManager.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }

            var employeesFromDb = await repositoryManager.Employee.GetEmployeesAsync(companyId, employeeParametrs, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(employeesFromDb.MetaData));

            var employeesDto = mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);
            return Ok(dataShaper.ShapeData(employeesDto, employeeParametrs.Fields));
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
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee)
        {
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
        [ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
        public async Task<IActionResult> DeleteEmloyeeForCompany(Guid companyId, Guid id)
        {
            var employeeForCompany = HttpContext.Items["employee"] as Employee;

            repositoryManager.Employee.DeleteEmployee(employeeForCompany);
            await repositoryManager.SaveAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
        public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee)
        {
            var employeeEntity = HttpContext.Items["employee"] as Employee;

            mapper.Map(employee, employeeEntity);
            await repositoryManager.SaveAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        [ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
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

            var employeeEntity = HttpContext.Items["employee"] as Employee;
            var employeeToPatch = mapper.Map<EmployeeForUpdateDto>(employeeEntity);

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
