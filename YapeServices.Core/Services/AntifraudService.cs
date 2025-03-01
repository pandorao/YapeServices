using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yape.Entities.Base;
using YapeServices.Core.Exceptions;
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

            try
            {
                if (transaction == null)
                {
                    throw new TransactionNotFoundException();
                }

                if (transaction.TransactionStatus != Entities.Enumerations.EnumTransactionStatus.Pending)
                {
                    throw new TransactionAlreadyExecutedException();
                }

                if (transaction.Value > 2000)
                {
                    throw new FraudDetectionException("Transaction value is greater than 2,000.");
                }

                if (await _transactionRepository.GetAcumulatedPerDayAsync(transaction.CreatedAt.Date) > 20000)
                {
                    throw new FraudDetectionException("Accumulated transaction value per day exceeds 20,000.");
                }
            }
            catch (ApplicationException ex)
            {
                serviceResult.AddModelError("Fraud", ex.Message);

                if (transaction == null)
                {
                    return serviceResult;
                }
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
