using System;
using System.Collections.Generic;

namespace Daneshkar_BC1403_BookStoreMVC.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Cover { get; set; } = null!;

    public int? ParentId { get; set; }

    public string DescriptionFa { get; set; } = null!;

    public string NameFa { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();

    public virtual ICollection<Category> InverseParent { get; set; } = new List<Category>();

    public virtual Category? Parent { get; set; }
}
