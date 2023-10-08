using CodeMonkeyBank.Application;
using CodeMonkeyBank.Domain;
using CodeMonkeyBank.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Text;

namespace CodeMonkeyBank.Controllers;

public class AccountController : Controller
{

    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Index()
    {
        var model = await GetAccountModel();
        return View(model);
    }

    [HttpGet]
    public async Task<ActionResult> Withdraw()
    {
        var model = await GetAccountModel();
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Withdraw(double? amount)
    {
        if (!amount.HasValue || amount <= 0)
        {
            ModelState.AddModelError("amount", "Please provide a valid amount.");
            return View();
        }

        var accountResult = await GetUserAccountAsync();

        if (accountResult.Result is OkObjectResult okResult)
        {
            var account = okResult.Value as AccountDTO;

            await _accountService.UpdateBalanceAsync(account.Username, amount.Value, isWithdrawal: true);

            return RedirectToAction("Withdraw", "Account");
        }
        else
        {
            return View();
        }
    }

    // GET: AccountController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: AccountController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(AccountModel accountModel)
    {
        try
        {
            if (ModelState.IsValid)
            {
                string? usernameValidationMessage = await _accountService.ValidateUsername(accountModel.Username);
                if (!string.IsNullOrEmpty(usernameValidationMessage))
                {
                    ModelState.AddModelError("Username", usernameValidationMessage);
                    return View();
                }

                var account = new Account
                {
                    FirstName = accountModel.FirstName,
                    LastName = accountModel.LastName,
                    BirthDate = accountModel.BirthDate,
                    Email = accountModel.Email,
                    Username = accountModel.Username,
                    Password = accountModel.Password,
                    Balance = accountModel.Balance
                };

                await _accountService.CreateAccountAsync(account);

                return RedirectToAction("Index", "Login");

            }

            return View();

        }
        catch
        {
            return View();
        }
    }
    [HttpGet]
    public async Task<ActionResult> Deposit()
    {
        var model = await GetAccountModel();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Deposit(double? amount)
    {
        if (!amount.HasValue || amount <= 0)
        {
            ModelState.AddModelError("amount", "Please provide a valid amount.");
            return View();
        }

        var accountResult = await GetUserAccountAsync();

        if (accountResult.Result is OkObjectResult okResult)
        {
            var account = okResult.Value as AccountDTO;

            await _accountService.UpdateBalanceAsync(account.Username, amount.Value, isWithdrawal: false);

            return RedirectToAction("Deposit", "Account");
        }
        else
        {
            return View();
        }
    }
    [HttpGet]
    [Route("api/accounts/user")]
    public async Task<ActionResult<AccountDTO>> GetUserAccountAsync()
    {

        var username = User.Identity.Name;
        var account = await _accountService.GetAccountByUsernameAsync(username);

        if (account == null) { return NotFound(); }

        return Ok(account);
    }
    private async Task<AccountModel> GetAccountModel()
    {
        var accountResult = await GetUserAccountAsync();

        if (accountResult.Result is OkObjectResult okResult)
        {
            var account = okResult.Value as AccountDTO;

            var model = new AccountModel
            {          
                FirstName = account.FirstName,
                LastName = account.LastName,
                BirthDate = account.BirthDate,
                Email = account.Email,
                Username = account.Username,
                Password = account.Password,
                Balance = account.Balance
            };

            return model;
        }
        else
        {
            // Handle Errors
            return new AccountModel();
        }
    }
}