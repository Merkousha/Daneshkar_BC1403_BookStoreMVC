using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Daneshkar_BC1403_BookStoreMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace Daneshkar_BC1403_BookStoreMVC.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly RefhubContext _context;

        public IndexModel(ILogger<IndexModel> logger, RefhubContext context)
        {
            _logger = logger;
            _context = context;
        }

        public List<Book> books { get; set; }
        public string something { get; set; }
        public async Task OnGetAsync()
        {
            books = await _context.Books.Where(x => x.Id < 50).ToListAsync();
        }
    }
}
