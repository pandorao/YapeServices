using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YapeServices.Entities.Enumerations;
using YapeServices.Entities.Models;
using YapeServices.Ports.Repositories;
using YapeServices.Services;

namespace YapeServices.Core.Tests
{
    public class AntifraudServiceTests
    {
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly AntifraudService _antifraudService;

        public AntifraudServiceTests()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _antifraudService = new AntifraudService(_transactionRepositoryMock.Object);
        }

        [Fact]
        public async Task ExecuteTransaction_TransactionNotFound_ShouldReturnError()
        {
            _transactionRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((Transaction)null);

            var result = await _antifraudService.ExecuteTransaction("123");

            Assert.False(result.Succeeded);
            Assert.Contains("Transaction not found", result.Errors["Fraud"]);
        }

        [Fact]
        public async Task ExecuteTransaction_TransactionAlreadyExecuted_ShouldReturnError()
        {
            var transaction = new Transaction { TransactionStatus = EnumTransactionStatus.Approved };
            _transactionRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(transaction);

            var result = await _antifraudService.ExecuteTransaction("123");

            Assert.False(result.Succeeded);
            Assert.Contains("Transaction was executed", result.Errors["Fraud"]);
        }

        [Fact]
        public async Task ExecuteTransaction_TransactionValueGreaterThan2000_ShouldReturnError()
        {
            var transaction = new Transaction { TransactionStatus = EnumTransactionStatus.Pending, Value = 2500 };
            _transactionRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(transaction);

            var result = await _antifraudService.ExecuteTransaction("123");

            Assert.False(result.Succeeded);
            Assert.Contains("Transaction value is greater than 2,000.", result.Errors["Fraud"]);
        }

        [Fact]
        public async Task ExecuteTransaction_AcumulatedPerDayGreaterThan20000_ShouldReturnError()
        {
            var transaction = new Transaction { TransactionStatus = EnumTransactionStatus.Pending, Value = 1500, CreatedAt = DateTime.Now };
            _transactionRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(transaction);
            _transactionRepositoryMock.Setup(repo => repo.GetAcumulatedPerDayAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(25000);

            var result = await _antifraudService.ExecuteTransaction("123");

            Assert.False(result.Succeeded);
            Assert.Contains("Accumulated transaction value per day exceeds 20,000.", result.Errors["Fraud"]);
        }

        [Fact]
        public async Task ExecuteTransaction_ValidTransaction_ShouldBeApproved()
        {
            var transaction = new Transaction { TransactionStatus = EnumTransactionStatus.Pending, Value = 1000, CreatedAt = DateTime.Now };
            _transactionRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(transaction);
            _transactionRepositoryMock.Setup(repo => repo.GetAcumulatedPerDayAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(10000);

            var result = await _antifraudService.ExecuteTransaction("123");

            Assert.True(result.Succeeded);
            Assert.Equal(EnumTransactionStatus.Approved, transaction.TransactionStatus);
        }
    }

}
