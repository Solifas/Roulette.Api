using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Roulette.Application.Commands.PlaceBet;
using Roulette.Application.Interfaces;
using Roulette.Domain;
using Roulette.Domain.Interfaces;
using Xunit;

namespace Roulette.Tests.Roulette.Application.Tests
{
    public class PlaceBetCommandHandler_Tests
    {
        [Fact]
        public async Task Handle_WhenCalledWithValidCommand_ShouldReturnBetId()
        {
            // Arrange
            var command = new PlaceBetCommand
            {
                UserName = "test",
                BetType = BetType.Even,
                Amount = 10
            };

            var betCreated = new Bet
            {
                Id = Guid.NewGuid().ToString(),
                BetType = BetType.Even,
                Amount = 10
            };
            var betHistory = new BetHistory();

            var mockBetEngine = new Mock<IBetEngine>();
            var mockBetHistoryRepo = new Mock<IRepository<BetHistory>>();
            var mockBetRepo = new Mock<IRepository<Bet>>();
            var mockUserRepo = new Mock<IRepository<User>>();

            mockBetEngine.Setup(x => x.PlaceBet(BetType.Even, It.IsAny<string>(), command.Amount)).Returns(betCreated);
            mockUserRepo.Setup(x => x.Get(It.IsAny<string>(), null)).ReturnsAsync(new User { Id = Guid.NewGuid().ToString(), UserName = command.UserName, Balance = 100M });

            var handler = new PlaceBetCommandHandler(mockBetEngine.Object, mockBetHistoryRepo.Object, mockBetRepo.Object, mockUserRepo.Object);

            // Act
            await handler.Handle(command, default);

            // Assert
            mockBetEngine.Verify(x => x.PlaceBet(BetType.Even, It.IsAny<Guid>().ToString(), command.Amount), Times.Once);
        }
    }
}