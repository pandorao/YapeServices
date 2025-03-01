using System;
using Yape.Entities.Base;
using YapeServices.Entities.Dtos.Transactions;
using YapeServices.Entities.Enumerations;
using YapeServices.Entities.Models;
using YapeServices.Ports.Messenger;
using YapeServices.Ports.Repositories;
using YapeServices.Ports.Services;

namespace YapeServices.Services
{
    public class TransactionServices : ITransactionsService
    {
        private ITransactionRepository _transactionRepository;
        private IMessengerProducerService _kafkaProducerService;

        public TransactionServices(
            ITransactionRepository transactionRepository,
            IMessengerProducerService kafkaProducerService)
        {
            _kafkaProducerService = kafkaProducerService;
            _transactionRepository = transactionRepository;
        }

        public async Task<ServiceResult<CreateTransactionResponse>> AddAsync(CreateTransactionRequest request)
        {
            var serviceResult = new ServiceResult<CreateTransactionResponse>();
            var model = new Transaction()
            {
                TransactionExternalId = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                SourceAccountId = request.SourceAccountId,
                TargetAccountId = request.TargetAccountId,
                TransferTypeId = request.TransferTypeId,
                Value = request.Value,
                ExecutedAt = null,
                TransactionStatus = EnumTransactionStatus.Pending,
            };

            if (!model.IsValid(out var erros))
            {
                serviceResult.AddModelError(erros);
                return serviceResult;
            }

            await _transactionRepository.AddAsync(model);

            serviceResult.ResponseObject = new CreateTransactionResponse()
            {
                CreatedAt = model.CreatedAt,
                TransactionExternalId = model.TransactionExternalId
            };
            await _kafkaProducerService
                .SendMessageAsync(null, model.TransactionExternalId);
            return serviceResult;
        }

        public async Task<List<Transaction>> GetAllAsync()
        {
           return await _transactionRepository.GetAllAsync();
        }

        public async Task<Transaction> GetByIdAsync(string id)
        {
            return await _transactionRepository.GetByIdAsync(id);
        }
    }
}
