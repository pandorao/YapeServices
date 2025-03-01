using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YapeServices.Entities.Dtos.Transactions;
using YapeServices.Entities.Models;
using YapeServices.Ports.Messenger;
using YapeServices.Ports.Repositories;
using YapeServices.Services;

namespace YapeServices.Core.Tests
{
    public class TransactionServicesTests
    {
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<IMessengerProducerService> _messengerProducerMock;
        private readonly TransactionServices _transactionServices;

        public TransactionServicesTests()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _messengerProducerMock = new Mock<IMessengerProducerService>();
            _transactionServices = new TransactionServices(
                _transactionRepositoryMock.Object,
                _messengerProducerMock.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldCreateTransaction_WhenValidRequest()
        {
            // Arrange
            var request = new CreateTransactionRequest
            {
                SourceAccountId = "123",
                TargetAccountId = "456",
                TransferTypeId = 1,
                Value = 1000
            };

            _transactionRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Transaction>()))
                .Returns(Task.CompletedTask);

            _messengerProducerMock.Setup(m => m.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _transactionServices.AddAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            Assert.NotNull(result.ResponseObject);
            _transactionRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Transaction>()), Times.Once);
            _messengerProducerMock.Verify(m => m.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnError_WhenTransactionIsInvalid()
        {
            // Arrange
            var request = new CreateTransactionRequest
            {
                SourceAccountId = "",
                TargetAccountId = "",
                TransferTypeId = 1,
                Value = -1000 // Valor inválido
            };

            // Act
            var result = await _transactionServices.AddAsync(request);

            // Assert
            Assert.False(result.Succeeded);
            Assert.NotEmpty(result.Errors);
            _transactionRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Transaction>()), Times.Never);
            _messengerProducerMock.Verify(m => m.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnTransactions()
        {
            // Arrange
            var transactions = new List<Transaction>
            {
                new Transaction { TransactionExternalId = "1", Value = 500 },
                new Transaction { TransactionExternalId = "2", Value = 1000 }
            };

            _transactionRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(transactions);

            // Act
            var result = await _transactionServices.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnTransaction_WhenTransactionExists()
        {
            // Arrange
            var transaction = new Transaction { TransactionExternalId = "1", Value = 500 };
            _transactionRepositoryMock.Setup(repo => repo.GetByIdAsync("1"))
                .ReturnsAsync(transaction);

            // Act
            var result = await _transactionServices.GetByIdAsync("1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1", result.TransactionExternalId);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenTransactionDoesNotExist()
        {
            // Arrange
            _transactionRepositoryMock.Setup(repo => repo.GetByIdAsync("1"))
                .ReturnsAsync((Transaction)null);

            // Act
            var result = await _transactionServices.GetByIdAsync("1");

            // Assert
            Assert.Null(result);
        }
    }
}
