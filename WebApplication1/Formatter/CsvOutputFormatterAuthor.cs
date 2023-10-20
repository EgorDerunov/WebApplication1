using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace WebApplication1.Formatter
{
    public class CsvOutputFormatterAuthor : TextOutputFormatter
    {
        public CsvOutputFormatterAuthor()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(Type type)
        {
            if (typeof(AuthorDto).IsAssignableFrom(type) ||
                typeof(IEnumerable<AuthorDto>).IsAssignableFrom(type))
            {
                return base.CanWriteType(type);
            }
            return false;
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            var buffer = new StringBuilder();

            if (context.Object is IEnumerable<AuthorDto>)
            {
                foreach (var author in (IEnumerable<AuthorDto>)context.Object)
                {
                    FormatCsv(buffer, author);
                }
            }
            else
            {
                FormatCsv(buffer, (AuthorDto)context.Object);
            }

            await response.WriteAsync(buffer.ToString());
        }

        private static void FormatCsv(StringBuilder buffer, AuthorDto author)
        {
            buffer.AppendLine($"{author.Id},\"{author.FullName},\"{author.DateBirth}\"");
        }
    }
}
