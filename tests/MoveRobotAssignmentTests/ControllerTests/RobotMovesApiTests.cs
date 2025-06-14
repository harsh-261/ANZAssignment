using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MoveRobotAssignment.Controllers;
using MoveRobotAssignment.Feature.LeftCommand;
using MoveRobotAssignment.Feature.MoveCommand;
using MoveRobotAssignment.Feature.MoveToPositionCommand;
using MoveRobotAssignment.Feature.ReportQuery;
using MoveRobotAssignment.Feature.RightCommand;

namespace MoveRobotAssignmentTests.ControllerTests;

public class RobotMovesApiTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly RobotMovesApi _controller;

    public RobotMovesApiTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new RobotMovesApi(_mediatorMock.Object);
    }

    [Fact]
    public async Task Move_ReturnsOkResult()
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<MoveCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _controller.Move();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(true, okResult.Value);
    }

    [Fact]
    public async Task Report_ReturnsOkResult()
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<ReportQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("Report");

        var result = await _controller.Report();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Report", okResult.Value);
    }

    [Fact]
    public async Task Right_ReturnsNoContent()
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<RightCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        var result = await _controller.Right();

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Left_ReturnsNoContent()
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<LeftCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        var result = await _controller.Left();

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task MoveToPosition_InvalidDirection_ReturnsBadRequest()
    {
        var result = await _controller.MoveToPosition(1, 2, "INVALID");

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("Invalid direction", badRequest.Value.ToString());
    }

    [Fact]
    public async Task MoveToPosition_ValidDirection_ReturnsOk()
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<MoveToPositionCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _controller.MoveToPosition(1, 2, "NORTH");

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(true, okResult.Value);
    }

    [Fact]
    public async Task MoveToInvalidPosition_ValidDirection_ReturnsFalse()
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<MoveToPositionCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _controller.MoveToPosition(6, 2, "EAST");

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(false, okResult.Value);
    }
}
