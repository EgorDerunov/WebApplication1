namespace Entities.DataTransferObjects.Book
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int YearIssue { get; set; }
        public string? Qenre { get; set; }
    }
}
