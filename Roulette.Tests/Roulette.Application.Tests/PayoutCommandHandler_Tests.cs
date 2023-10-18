using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Roulette.Application.Commands.Payout;
using Roulette.Application.Exceptions;
using Roulette.Application.Interfaces;
using Roulette.Domain;
using Xunit;

namespace Roulette.Tests.Roulette.Application.Tests
{
    public class PayoutCommandHandler_Tests
    {
        [Fact]
        public async Task PayoutCommandHandler_Success()
        {
            var mockUserRepository = new Mock<IRepository<User>>();
            var mockPayOutRepository = new Mock<IRepository<PayOut>>();

            // Arrange
            var command = new PayoutCommand
            {
                BetId = Guid.NewGuid().ToString(),
                UserName = "TestUser",
                Amount = 100M
            };
            var handler = new PayoutCommandHandler(mockUserRepository.Object, mockPayOutRepository.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(command.BetId, result.BetId);
            Assert.Equal(command.Amount, result.Amount);
        }

        [Fact]
        public async Task PayoutCommandHandler_ThrowsValidationException_WhenUserNameIsEmpty()
        {
            // Arrange
            var mockUserRepository = new Mock<IRepository<User>>();
            var mockPayOutRepository = new Mock<IRepository<PayOut>>();
            var command = new PayoutCommand
            {
                BetId = Guid.NewGuid().ToString(),
                UserName = "",
                Amount = 100M
            };
            var handler = new PayoutCommandHandler(mockUserRepository.Object, mockPayOutRepository.Object);

            // Act
            var exception = await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, default));

            // Assert
            Assert.Equal("UserName is required.", exception.ValidationErrors.FirstOrDefault());
        }
    }
}