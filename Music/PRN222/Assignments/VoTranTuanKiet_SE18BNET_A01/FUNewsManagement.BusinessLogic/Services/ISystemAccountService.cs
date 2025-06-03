using FUNewsManagement.Models;

namespace FUNewsManagement.BusinessLogic.Services
{
    public interface ISystemAccountService
    {
        Task<IEnumerable<SystemAccount>> GetAllAccountsAsync();
        Task<SystemAccount?> GetAccountByIdAsync(short id);
        Task<SystemAccount?> GetAccountByEmailAsync(string email);
        Task<SystemAccount?> AuthenticateAsync(string email, string password);
        Task<SystemAccount> CreateAccountAsync(SystemAccount account);
        Task<SystemAccount> UpdateAccountAsync(SystemAccount account);
        Task<bool> DeleteAccountAsync(short id);
        Task<bool> AccountExistsAsync(short id);
        Task<bool> EmailExistsAsync(string email, short? excludeId = null);
        Task<IEnumerable<SystemAccount>> SearchAccountsAsync(string searchTerm);
        Task<bool> ValidateAccountDataAsync(SystemAccount account, bool isUpdate = false);
    }
}
