﻿using Microsoft.AspNetCore.Mvc;

namespace NetCoreEF.Areas.Admin.Controllers
{
  [Area("Admin")]
  public class HomeController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }
  }
}
