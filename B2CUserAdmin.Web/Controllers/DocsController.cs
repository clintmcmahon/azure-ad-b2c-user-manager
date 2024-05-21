using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace B2CUserAdmin.Web.Controllers;

[AllowAnonymous]
public class DocsController : Controller
{
    private readonly ILogger<DocsController> _logger;

    public DocsController(ILogger<DocsController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
}
