using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yape.Adapters.Services.Repositories.Persistance;
using YapeServices.Entities.Models;
using YapeServices.Ports.Repositories;

namespace YapeServices.Database.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Transaction>> GetAllAsync()
        {
            return await _context.Transactions.ToListAsync();
        }

        public async Task<Transaction> GetByIdAsync(string id)
        {
            return await _context
                .Transactions
                .FirstOrDefaultAsync(x => x.TransactionExternalId == id);
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
        }
    }
}
