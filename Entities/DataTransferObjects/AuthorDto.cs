﻿namespace Entities.DataTransferObjects
{
    public class AuthorDto
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public string? DateBirth { get; set; }
    }
}
