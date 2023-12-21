using AutoMapper;
using Entities.DataTransferObjects.Author;
using Entities.DataTransferObjects.Book;
using Entities.DataTransferObjects.Company;
using Entities.DataTransferObjects.Employee;
using Entities.Models;

namespace WebApplication1.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>()
                .ForMember(c => c.FullAddress,
                opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));

            CreateMap<Author, AuthorDto>()
                .ForMember(c => c.FullName,
                opt => opt.MapFrom(x => string.Join(' ', x.Name, x.Surname)));

            CreateMap<Employee, EmployeeDto>();
            CreateMap<Book, BookDto>();

            CreateMap<CompanyForCreationDto, Company>();
            CreateMap<EmployeeForCreationDto, Employee>();
            CreateMap<AuthorForCreationDto, Author>();
            CreateMap<BookForCreationDto, Book>();

            CreateMap<CompanyForUpdateDto, Company>();
            CreateMap<EmployeeForUpdateDto, Employee>();
            CreateMap<AuthorForUpdateDto, Author>();
            CreateMap<BookForUpdateDto, Book>();
        }
    }
}
