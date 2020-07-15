using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Admin.Users
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public IndexModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        
        public async Task OnGetAsync()
        {
            var managers = await _userManager.GetUsersInRoleAsync(Constants.ManagersRole);
            var analysts = await _userManager.GetUsersInRoleAsync(Constants.AnalystsRole);
            Users = managers.Union(analysts).ToList();
        }

#nullable disable
        public IList<User> Users { get;set; }
    }
}
