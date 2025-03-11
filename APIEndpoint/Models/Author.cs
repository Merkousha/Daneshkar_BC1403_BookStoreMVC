using System;
using System.Collections.Generic;
using APIEndpoint.ModelBinders;
using Microsoft.AspNetCore.Mvc;

namespace APIEndpoint.Models;

[ModelBinder(BinderType = typeof(AuthorModelBinder))]
public partial class Author
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Cover { get; set; } = null!;

    public virtual ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
}
