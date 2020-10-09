using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ExporterWeb.Areas.Identity.Pages.Account.RegisterModel;

namespace ExporterWeb.Components
{
    public class Register : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var registerViewModel = new InputModel();
            return View("_Register", registerViewModel);
        }
    }
}
