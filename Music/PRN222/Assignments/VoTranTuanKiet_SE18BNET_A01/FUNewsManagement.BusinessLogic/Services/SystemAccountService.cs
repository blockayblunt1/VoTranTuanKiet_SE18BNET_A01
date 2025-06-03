using FUNewsManagement.DataAccess.Repositories;
using FUNewsManagement.Models;

namespace FUNewsManagement.BusinessLogic.Services
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly ISystemAccountRepository _accountRepository;

        public SystemAccountService(ISystemAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IEnumerable<SystemAccount>> GetAllAccountsAsync()
        {
            return await _accountRepository.GetAllAsync();
        }

        public async Task<SystemAccount?> GetAccountByIdAsync(short id)
        {
            return await _accountRepository.GetByIdAsync(id);
        }

        public async Task<SystemAccount?> GetAccountByEmailAsync(string email)
        {
            return await _accountRepository.GetByEmailAsync(email);
        }

        public async Task<SystemAccount?> AuthenticateAsync(string email, string password)
        {
            return await _accountRepository.GetByEmailAndPasswordAsync(email, password);
        }

        public async Task<SystemAccount> CreateAccountAsync(SystemAccount account)
        {
            if (!await ValidateAccountDataAsync(account))
            {
                throw new ArgumentException("Invalid account data");
            }

            if (await _accountRepository.EmailExistsAsync(account.AccountEmail))
            {
                throw new ArgumentException("Email already exists");
            }

            return await _accountRepository.AddAsync(account);
        }

        public async Task<SystemAccount> UpdateAccountAsync(SystemAccount account)
        {
            if (!await ValidateAccountDataAsync(account, true))
            {
                throw new ArgumentException("Invalid account data");
            }

            if (await _accountRepository.EmailExistsAsync(account.AccountEmail, account.AccountId))
            {
                throw new ArgumentException("Email already exists");
            }

            return await _accountRepository.UpdateAsync(account);
        }

        public async Task<bool> DeleteAccountAsync(short id)
        {
            return await _accountRepository.DeleteAsync(id);
        }

        public async Task<bool> AccountExistsAsync(short id)
        {
            return await _accountRepository.ExistsAsync(id);
        }

        public async Task<bool> EmailExistsAsync(string email, short? excludeId = null)
        {
            return await _accountRepository.EmailExistsAsync(email, excludeId);
        }

        public async Task<IEnumerable<SystemAccount>> SearchAccountsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllAccountsAsync();
            }

            return await _accountRepository.SearchAsync(searchTerm);
        }

        public async Task<bool> ValidateAccountDataAsync(SystemAccount account, bool isUpdate = false)
        {
            if (account == null) return false;
            if (string.IsNullOrWhiteSpace(account.AccountName)) return false;
            if (string.IsNullOrWhiteSpace(account.AccountEmail)) return false;
            if (string.IsNullOrWhiteSpace(account.AccountPassword)) return false;
            if (account.AccountRole != 1 && account.AccountRole != 2) return false; // 1: Staff, 2: Lecturer

            // Email format validation
            if (!IsValidEmail(account.AccountEmail)) return false;

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
