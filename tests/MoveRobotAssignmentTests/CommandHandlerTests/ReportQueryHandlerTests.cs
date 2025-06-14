using Moq;
using MoveRobotAssignment.Feature.ReportQuery;
using MoveRobotAssignment.StateMachine.Interface;

namespace MoveRobotAssignmentTests.CommandHandlerTests;

public class ReportQueryHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsReportString()
    {
        // Arrange
        var expectedReport = "2,3,NORTH";
        var boardMock = new Mock<IBoard>();
        boardMock.Setup(b => b.Report()).Returns(expectedReport);

        var handler = new ReportQueryHandler(boardMock.Object);
        var query = new ReportQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(expectedReport, result);
        boardMock.Verify(b => b.Report(), Times.Once);
    }
}
