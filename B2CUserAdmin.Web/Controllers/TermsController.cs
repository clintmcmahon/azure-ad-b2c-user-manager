using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace B2CUserAdmin.Web.Controllers;

[AllowAnonymous]
public class TermsController : Controller
{
    private readonly ILogger<TermsController> _logger;

    public TermsController(ILogger<TermsController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
}
