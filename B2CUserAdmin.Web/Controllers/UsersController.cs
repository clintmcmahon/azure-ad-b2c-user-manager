using Microsoft.AspNetCore.Mvc;
using B2CUserAdmin.Web.Services;
using B2CUserAdmin.Models;
namespace B2CUserAdmin.Web.Controllers;

public class UsersController : Controller
{
    private B2CUsersService _graphAPIService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IConfiguration configuration,
    ILogger<UsersController> logger, B2CUsersService graphAPIService)
    {
        _configuration = configuration;
        _logger = logger;
        _graphAPIService = graphAPIService;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _graphAPIService.GetUsersAsync();
        return View(users);
    }

    public async Task<IActionResult> Details(string id)
    {

        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var model = new DetailUserViewModel();

        try
        {
            var user = await _graphAPIService.GetUserAllProperties(id);
            var customAttributes = await _graphAPIService.GetCustomExtensionAttributes();

            if (user == null)
            {
                return NotFound();
            }

            model.User = user;
            model.CustomAttributes = customAttributes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {

        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        try
        {
            var user = await _graphAPIService.GetUserAllProperties(id);
            var customAttributes = await _graphAPIService.GetCustomExtensionAttributes();

            if (user == null)
            {
                return NotFound();
            }

            var model = new EditUserViewModel
            {
                // Directly map properties from user to the model
                Id = user.Id,
                DisplayName = user.DisplayName,
                GivenName = user.GivenName,
                Surname = user.Surname,
                AccountEnabled = user.AccountEnabled,
                Mail = user.Mail,
                MobilePhone = user.MobilePhone,
                FaxNumber = user.FaxNumber,
                JobTitle = user.JobTitle,
                CompanyName = user.CompanyName,
                Department = user.Department,
                EmployeeId = user.EmployeeId,
                EmployeeType = user.EmployeeType,
                OfficeLocation = user.OfficeLocation,
                CustomAttributes = customAttributes
            };

            var userAdditionalData = user.AdditionalData;
            var additionalDataAsString = new Dictionary<string, string>();

            foreach (var item in userAdditionalData)
            {
                additionalDataAsString[item.Key] = item.Value?.ToString();
            }

            model.AdditionalData = additionalDataAsString;

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> Edit(string id, EditUserViewModel model)
    {
        try
        {
            var user = new Microsoft.Graph.Models.User
            {
                Id = model.Id,
                DisplayName = model.DisplayName,
                GivenName = model.GivenName,
                Surname = model.Surname,
                AccountEnabled = model.AccountEnabled ?? false,
                Mail = model.Mail,
                MobilePhone = model.MobilePhone,
                FaxNumber = model.FaxNumber,
                JobTitle = model.JobTitle,
                CompanyName = model.CompanyName,
                Department = model.Department,
                EmployeeId = model.EmployeeId,
                EmployeeType = model.EmployeeType,
                OfficeLocation = model.OfficeLocation
            };

            foreach (var additionalItem in model.AdditionalData)
            {
                user.AdditionalData[additionalItem.Key] = additionalItem.Value;
            }

            await _graphAPIService.UpdateUserAsync(id, user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        return View();
    }
}
