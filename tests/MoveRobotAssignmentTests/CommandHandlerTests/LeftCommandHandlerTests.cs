using MediatR;
using Moq;
using MoveRobotAssignment.Feature.LeftCommand;
using MoveRobotAssignment.StateMachine.Interface;

namespace MoveRobotAssignmentTests.CommandHandlerTests;

public class LeftCommandHandlerTests
{
    [Fact]
    public async Task Handle_CallsBoardTurnLeft_ReturnsUnit()
    {
        // Arrange
        var boardMock = new Mock<IBoard>();
        boardMock.Setup(b => b.TurnLeft());

        var handler = new LeftCommandHandler(boardMock.Object);
        var command = new LeftCommand();

        // Act
        var result = await ((IRequestHandler<LeftCommand, Unit>)handler).Handle(command, CancellationToken.None);

        // Assert
        boardMock.Verify(b => b.TurnLeft(), Times.Once);
        Assert.Equal(Unit.Value, result);
    }
}
