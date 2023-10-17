using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Roulette.Application.Commands.Spin;
using Roulette.Application.Interfaces;
using Roulette.Domain;
using Roulette.Domain.Interfaces;
using Xunit;

namespace Roulette.Tests.Roulette.Application.Tests
{
    public class SpinCommandHandler_Tests
    {
        [Fact]
        public async Task SpinCommandHandler_ShouldReturnSpinResponse()
        {
            // Arrange
            var spinCommand = new SpinCommand();
            var mockBetEngine = new Mock<IBetEngine>();
            var mockSpinHistoryRepository = new Mock<IRepository<SpinHistory>>();
            var spinCommandHandler = new SpinCommandHandler(mockBetEngine.Object, mockSpinHistoryRepository.Object);

            mockBetEngine.Setup(x => x.Spin()).ReturnsAsync(BetType.Even);
            // Act
            var spinResponse = await spinCommandHandler.Handle(spinCommand, default);

            // Assert
            Assert.Equal(BetType.Even, spinResponse);
            Assert.IsType<BetType>(spinResponse);
        }
    }
}