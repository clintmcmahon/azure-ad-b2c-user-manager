using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace B2CUserAdmin.Web.Controllers;

[AllowAnonymous]
public class PrivacyController : Controller
{
    private readonly ILogger<PrivacyController> _logger;

    public PrivacyController(ILogger<PrivacyController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
}
