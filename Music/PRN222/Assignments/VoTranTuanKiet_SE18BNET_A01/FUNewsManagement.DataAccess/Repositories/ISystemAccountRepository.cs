using FUNewsManagement.Models;

namespace FUNewsManagement.DataAccess.Repositories
{
    public interface ISystemAccountRepository
    {
        Task<IEnumerable<SystemAccount>> GetAllAsync();
        Task<SystemAccount?> GetByIdAsync(short id);
        Task<SystemAccount?> GetByEmailAsync(string email);
        Task<SystemAccount?> GetByEmailAndPasswordAsync(string email, string password);
        Task<SystemAccount> AddAsync(SystemAccount account);
        Task<SystemAccount> UpdateAsync(SystemAccount account);
        Task<bool> DeleteAsync(short id);
        Task<bool> ExistsAsync(short id);
        Task<bool> EmailExistsAsync(string email, short? excludeId = null);
        Task<IEnumerable<SystemAccount>> SearchAsync(string searchTerm);
    }
}
