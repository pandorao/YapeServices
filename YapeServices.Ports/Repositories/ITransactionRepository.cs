using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YapeServices.Entities.Models;

namespace YapeServices.Ports.Repositories
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetAllAsync();
        Task<Transaction> GetByIdAsync(string id);
        Task AddAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
    }
}
