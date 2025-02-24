using System;
using System.Collections.Generic;

namespace APIEndpoint.Models;

public partial class Author
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Cover { get; set; } = null!;

    public virtual ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
}
