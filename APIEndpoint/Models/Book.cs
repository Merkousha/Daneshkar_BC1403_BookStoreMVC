using System;
using System.Collections.Generic;

namespace APIEndpoint.Models;

public partial class Book
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Cover { get; set; } = null!;

    public string? Document { get; set; }

    public int SeriId { get; set; }

    public string? Author { get; set; }

    public string? Publisher { get; set; }

    public string Slug { get; set; } = null!;

    public int? CategoryId { get; set; }

    public int? LanguageId { get; set; }

    public int? SkillLevelId { get; set; }

    public string? Pages { get; set; }

    public string? Size { get; set; }

    public string? Year { get; set; }

    public string Isbn { get; set; } = null!;

    public string? Mirror1 { get; set; }

    public string? Mirror2 { get; set; }

    public int ViewCount { get; set; }

    public int AvgRating { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public int DownloadCount { get; set; }

    public int Featured { get; set; }

    public string Thumb { get; set; } = null!;

    public string DescriptionFa { get; set; } = null!;

    public string DescriptionEn { get; set; } = null!;

    public int CopyrightLimited { get; set; }

    public virtual ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();

    public virtual Category? Category { get; set; }
}
