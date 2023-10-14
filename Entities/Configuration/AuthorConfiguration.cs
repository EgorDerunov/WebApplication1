using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Xml.Linq;

namespace Entities.Configuration
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasData
            (
                new Author
                {
                    Id = new Guid("64ace7ab-26a0-4429-8d8c-2209d83e9c62"),
                    Name = "Egor",
                    Surname = "Derunov",
                    DateBirth = "30.06.2004"
                },
                new Author
                {
                    Id = new Guid("75e02071-4510-4f5a-a80a-8545f6470a06"),
                    Name = "Ivan",
                    Surname = "Ivanov",
                    DateBirth = "01.01.2001"
                }
             ); 
        }
    }
}
