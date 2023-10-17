//create a unit test for the CreateUserCommandHandler class

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Roulette.Application.Commands.CreateUser;
using Roulette.Application.Exceptions;
using Roulette.Application.Interfaces;
using Roulette.Domain;
using Xunit;

public class CreateUserCommandHandler_Tests
{
    [Fact]
    public async Task CreateUserCommandHandler_AddsUserToRepository()
    {
        // Arrange
        var mockRepository = new Mock<IRepository<User>>();
        var command = new CreateUserCommand
        {
            UserName = "TestUser",
            Balance = 1000M
        };
        var handler = new CreateUserCommandHandler(mockRepository.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        mockRepository.Verify(x => x.AddAsync(It.IsAny<string>(), It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task CreateUserCommandHandler_ThrowsValidationException_WhenUserNameIsEmpty()
    {
        // Arrange
        var mockRepository = new Mock<IRepository<User>>();
        var command = new CreateUserCommand
        {
            UserName = "",
            Balance = 1000M
        };
        var handler = new CreateUserCommandHandler(mockRepository.Object);

        // Act
        var exception = await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));

        // Assert
        Assert.Equal("UserName is required.", exception.ValidationErrors.FirstOrDefault());
    }
}