using System;
using System.Collections.Generic;

namespace Daneshkar_BC1403_BookStoreMVC.Models;

public partial class BookAuthor
{
    public int Id { get; set; }

    public int RefrenceId { get; set; }

    public int AuthorId { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual Book Refrence { get; set; } = null!;
}
