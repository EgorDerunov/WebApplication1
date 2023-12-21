using Entities.Models;
using Repository.Extensions.Utility;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic;

namespace Repository.Extensions
{
    public static class RepositoryBookExtensions
    {
        public static IQueryable<Book> FilterEmployees(this IQueryable<Book> books, uint minYear, uint maxYear) =>
            books.Where(e => (e.YearIssue >= minYear && e.YearIssue <= maxYear));

        public static IQueryable<Book> Search(this IQueryable<Book> books, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return books;
            }

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return books.Where(e => e.Name.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Book> Sort(this IQueryable<Book> books, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return books.OrderBy(e => e.Name);
            }

            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(Book).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                {
                    continue;
                }

                var propertyFromQueryName = param.Split(' ')[0];
                var objectProperty = propertyInfos
                    .FirstOrDefault(pi => pi.Name
                    .Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty == null)
                {
                    continue;
                }

                var direction = param.EndsWith(" desc") ? "descending" : "ascending";
                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction},");
            }


            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Book>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
            {
                return books.OrderBy(e => e.Name);
            }

            return books.OrderBy(orderQuery);
        }
    }
}
