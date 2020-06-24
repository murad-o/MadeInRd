using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace ExporterWeb.Pages
{
    [AllowAnonymous]
    public class _TestModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public _TestModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Dummy>? Dummies { get; set; }
        public void OnGet()
        {
            Dummies = _dbContext.Dummies.ToList();
        }
    }
}