using MediatR;
using Moq;
using MoveRobotAssignment.Feature.RightCommand;
using MoveRobotAssignment.StateMachine.Interface;

namespace MoveRobotAssignmentTests.CommandHandlerTests;

public class RightCommandHandlerTests
{
    [Fact]
    public async Task Handle_CallsBoardTurnRight_ReturnsUnit()
    {
        // Arrange
        var boardMock = new Mock<IBoard>();
        boardMock.Setup(b => b.TurnRight());

        var handler = new RightCommandHandler(boardMock.Object);
        var command = new RightCommand();

        // Act
        var result = await ((IRequestHandler<RightCommand, Unit>)handler).Handle(command, CancellationToken.None);

        // Assert
        boardMock.Verify(b => b.TurnRight(), Times.Once);
        Assert.Equal(Unit.Value, result);
    }
}
