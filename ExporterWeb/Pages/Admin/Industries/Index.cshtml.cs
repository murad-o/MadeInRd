﻿using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ExporterWeb.Pages.Admin.Industries
{
    [Authorize(Roles = Constants.AdministratorsRole)]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            Industry =  await _context.IndustryTranslations!.Include(i => i.Industry).Where(i =>
                i.Language == CultureInfo.CurrentCulture.TwoLetterISOLanguageName).ToListAsync();
        }
        
        #nullable disable
        public IList<IndustryTranslation> Industry { get; set; }
    }
}