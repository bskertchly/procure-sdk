using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebSample.Models;
using WebSample.Services;

namespace WebSample.Controllers;

public class HomeController : Controller
{
    private readonly AuthenticationService _authService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(AuthenticationService authService, ILogger<HomeController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var isAuthenticated = await _authService.IsAuthenticatedAsync();
        var model = new HomeViewModel
        {
            IsAuthenticated = isAuthenticated
        };

        if (isAuthenticated)
        {
            var token = await _authService.GetCurrentTokenAsync();
            if (token != null)
            {
                model.TokenExpiresAt = token.ExpiresAt;
                model.TokenScopes = token.Scopes;
            }
        }

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}