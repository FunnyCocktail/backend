﻿using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Cocktail : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ImageUri { get; set; }

        public Guid AuthorId { get; set; }

        public User? Author { get; set; }
    }
}
