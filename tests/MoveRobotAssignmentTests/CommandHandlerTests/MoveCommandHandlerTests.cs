using Moq;
using MoveRobotAssignment.Feature.MoveCommand;
using MoveRobotAssignment.StateMachine.Interface;

namespace MoveRobotAssignmentTests.CommandHandlerTests;

public class MoveCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenBoardMoveReturnsTrue_ReturnsTrue()
    {
        // Arrange
        var boardMock = new Mock<IBoard>();
        boardMock.Setup(b => b.Move()).Returns(true);

        var handler = new MoveCommandHandler(boardMock.Object);
        var command = new MoveCommand();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        boardMock.Verify(b => b.Move(), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenBoardMoveReturnsFalse_ReturnsFalse()
    {
        // Arrange
        var boardMock = new Mock<IBoard>();
        boardMock.Setup(b => b.Move()).Returns(false);

        var handler = new MoveCommandHandler(boardMock.Object);
        var command = new MoveCommand();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        boardMock.Verify(b => b.Move(), Times.Once);
    }
}
