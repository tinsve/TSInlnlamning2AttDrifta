using Amazon.Auth.AccessControlPolicy;
using CodeMonkeyBank.Application;
using CodeMonkeyBank.Domain;
using CodeMonkeyBank.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Core.Authentication;
using System;
using System.Security.Claims;
using System.Security.Principal;

namespace CodeMonkeyBank.Controllers;

public class LoginController : Controller
{
    private readonly IAccountService _accountService;
    private readonly IAccountRepository _accountRepository;
    private readonly IPasswordHashService _passwordHashService;

    public LoginController(IAccountService accountService, IAccountRepository accountRepository, IPasswordHashService passwordHashService)
    {
        _accountService = accountService;
        _accountRepository = accountRepository;
        _passwordHashService = passwordHashService;
    }

    public ActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Index(LoginModel loginModel)
    {

        if (!ModelState.IsValid)
        {
            return View(loginModel);
        }

        if (await loginModel.ValidateLoginAsync(_accountRepository, _passwordHashService))
        {
            // Set up the session/cookie for the authenticated user.
            var claims = new[] { new Claim(ClaimTypes.Name, loginModel.Username) };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Account");
        }
        else
        {
            ViewBag.ErrorMessage = "Invalid username or password.";
            return View();
        }
    }
    [Authorize]
    public IActionResult Logout()
    {
        return SignOut(
        new AuthenticationProperties
        {
            RedirectUri = Url.Action("Index", "Login")
        },
        CookieAuthenticationDefaults.AuthenticationScheme);
    }
}