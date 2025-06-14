using Moq;
using MoveRobotAssignment.Enums;
using MoveRobotAssignment.Feature.MoveToPositionCommand;
using MoveRobotAssignment.StateMachine.Interface;

namespace MoveRobotAssignmentTests.CommandHandlerTests;

public class MoveToPositionCommandHandlerTests
{
    [Fact]
    public async Task Handle_PlaceReturnsTrue_ReturnsTrue()
    {
        // Arrange
        var boardMock = new Mock<IBoard>();
        Direction direction = Direction.NORTH;
        boardMock.Setup(b => b.Place(1, 2, direction)).Returns(true);

        var handler = new MoveToPositionCommandHandler(boardMock.Object);
        var command = new MoveToPositionCommand(1, 2, direction);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        boardMock.Verify(b => b.Place(1, 2, direction), Times.Once);
    }

    [Fact]
    public async Task Handle_PlaceReturnsFalse_ReturnsFalse()
    {
        // Arrange
        var boardMock = new Mock<IBoard>();
        Direction direction = Direction.SOUTH;
        boardMock.Setup(b => b.Place(0, 0, direction)).Returns(false);

        var handler = new MoveToPositionCommandHandler(boardMock.Object);
        var command = new MoveToPositionCommand(6, 0, direction);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        boardMock.Verify(b => b.Place(6, 0, direction), Times.Once);
    }
}
