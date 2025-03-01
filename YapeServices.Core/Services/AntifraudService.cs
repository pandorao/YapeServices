using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yape.Entities.Base;
using YapeServices.Entities.Enumerations;
using YapeServices.Ports.Repositories;
using YapeServices.Ports.Services;

namespace YapeServices.Services
{
    public class AntifraudService : IAntifraudService
    {
        private ITransactionRepository _transactionRepository;
        public AntifraudService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<ServiceResult> ExecuteTransaction(string id)
        {
            var serviceResult = new ServiceResult();

            var transaction = await _transactionRepository
                .GetByIdAsync(id);
            if (transaction == null)
            {
                serviceResult.AddModelError("", "Transaction not found");
                return serviceResult;
            }

            if (transaction.TransactionStatus != Entities.Enumerations.EnumTransactionStatus.Pending)
            {
                serviceResult.AddModelError("", "Transaction was executed");
                return serviceResult;
            }

            if (transaction.Value > 2000)
            {
                serviceResult.AddModelError("", "Transaction value greater than 2.000");
            }

            if (await _transactionRepository.GetAcumulatedPerDayAsync(transaction.CreatedAt.Date) > 20000)
            {
                serviceResult.AddModelError("", "Acumulated per day is greater than 20.000");
            }

            transaction.TransactionStatus = serviceResult.Succeeded ?
                EnumTransactionStatus.Approved :
                EnumTransactionStatus.Rejected;
            transaction.ExecutedAt = DateTime.Now;
            transaction.AntifraudReason = serviceResult.Succeeded ?
                "Appoved" :
                $"Rejected: {string.Join(", ", serviceResult.Errors.SelectMany(x => x.Value))}";

            await _transactionRepository.UpdateAsync(transaction);
            return serviceResult;
        }
    }
}
