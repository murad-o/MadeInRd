using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ExporterWeb.Models;

namespace ExporterWeb.Pages.Admin.AboutRegion
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync(string lang = "ru")
        {
            AboutRegionModel = await _context.AboutRegionContents!.FirstOrDefaultAsync(x => x.Lang == lang);
        }

        #nullable disable
        public AboutRegionModel AboutRegionModel { get; set; }
    }
}
