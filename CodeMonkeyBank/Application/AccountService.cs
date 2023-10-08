using CodeMonkeyBank.Domain;
using MongoDB.Bson;


namespace CodeMonkeyBank.Application
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordHashService _passwordHashService;
        public AccountService(IAccountRepository accountRepository, IPasswordHashService passwordHashService)
        {
            _accountRepository = accountRepository;
            _passwordHashService = passwordHashService;
        }

        public async Task CreateAccountAsync(Account account)
        {
            account.Password = _passwordHashService.HashPassword(account.Password);
            await _accountRepository.CreateAccountAsync(account);
        }

        public async Task<AccountDTO> GetAccountAsync(ObjectId id)
        {
            var account = await _accountRepository.GetAccountByIdAsync(id);

            // Convert Account to AccountDTO

            return new AccountDTO
            {
                Id = account.AccountId,
                FirstName = account.FirstName,
                LastName = account.LastName,
                BirthDate = account.BirthDate,
                Email = account.Email,
                Username = account.Username,
                Password = account.Password,
                Balance = account.Balance
            };
        }

        public async Task<AccountDTO> GetAccountByUsernameAsync(string? username)
        {
            var account = await _accountRepository.GetAccountByUsernameAsync(username);

            return new AccountDTO
            {
                Id = account.AccountId,
                FirstName = account.FirstName,
                LastName = account.LastName,
                BirthDate = account.BirthDate,
                Email = account.Email,
                Username = account.Username,
                Password = account.Password,
                Balance = account.Balance
            };
        }

        public async Task UpdateBalanceAsync(string? username, double amount, bool isWithdrawal)
        {
            var account = await _accountRepository.GetAccountByUsernameAsync(username);
            if (account != null)
            {
                if (isWithdrawal)
                {
                    if (Math.Abs(amount) <= account.Balance)
                    {
                        account.Balance -= Math.Abs(amount);
                    }
                    else
                    {
                        // Handle insufficient balance for withdrawal
                        throw new InvalidOperationException("Insufficient balance for withdrawal.");
                    }
                }
                else
                {
                    account.Balance += amount;
                }

                await _accountRepository.UpdateAsync(account);
            }
        }


        public async Task<string?> ValidateUsername(string? username)
        {

            if (string.IsNullOrEmpty(username))
            {
                return "Username field is empty. Please provide a username.";
            }

            var user = await _accountRepository.GetAccountByUsernameAsync(username);

            if(user != null && username == user.Username) 
            {
                return "This username is already taken.";
            }

            // If no Validation error we return null
            return null;
        }
    }
}

