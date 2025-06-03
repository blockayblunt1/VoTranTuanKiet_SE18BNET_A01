using Microsoft.EntityFrameworkCore;
using FUNewsManagement.Models;

namespace FUNewsManagement.DataAccess.Repositories
{
    public class SystemAccountRepository : ISystemAccountRepository
    {
        private readonly FUNewsManagementDbContext _context;

        public SystemAccountRepository(FUNewsManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SystemAccount>> GetAllAsync()
        {
            return await _context.SystemAccounts.ToListAsync();
        }

        public async Task<SystemAccount?> GetByIdAsync(short id)
        {
            return await _context.SystemAccounts.FindAsync(id);
        }

        public async Task<SystemAccount?> GetByEmailAsync(string email)
        {
            return await _context.SystemAccounts
                .FirstOrDefaultAsync(a => a.AccountEmail == email);
        }

        public async Task<SystemAccount?> GetByEmailAndPasswordAsync(string email, string password)
        {
            return await _context.SystemAccounts
                .FirstOrDefaultAsync(a => a.AccountEmail == email && a.AccountPassword == password);
        }

        public async Task<SystemAccount> AddAsync(SystemAccount account)
        {
            _context.SystemAccounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<SystemAccount> UpdateAsync(SystemAccount account)
        {
            _context.SystemAccounts.Update(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<bool> DeleteAsync(short id)
        {
            var account = await _context.SystemAccounts.FindAsync(id);
            if (account == null) return false;

            _context.SystemAccounts.Remove(account);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(short id)
        {
            return await _context.SystemAccounts.AnyAsync(a => a.AccountId == id);
        }

        public async Task<bool> EmailExistsAsync(string email, short? excludeId = null)
        {
            var query = _context.SystemAccounts.Where(a => a.AccountEmail == email);
            if (excludeId.HasValue)
            {
                query = query.Where(a => a.AccountId != excludeId.Value);
            }
            return await query.AnyAsync();
        }

        public async Task<IEnumerable<SystemAccount>> SearchAsync(string searchTerm)
        {
            return await _context.SystemAccounts
                .Where(a => a.AccountName.Contains(searchTerm) || a.AccountEmail.Contains(searchTerm))
                .ToListAsync();
        }
    }
}
