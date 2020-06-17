using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExporterWeb.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExporterWeb.Pages
{
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