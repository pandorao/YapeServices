using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yape.Entities.Base;
using YapeServices.Entities.Dtos.Transactions;
using YapeServices.Entities.Models;

namespace YapeServices.Ports.Services
{
    public interface ITransactionsService
    {
        Task<List<Transaction>> GetAllAsync();
        Task<Transaction> GetByIdAsync(string id);
        Task<ServiceResult<CreateTransactionResponse>> AddAsync(CreateTransactionRequest transaction);
    }
}
