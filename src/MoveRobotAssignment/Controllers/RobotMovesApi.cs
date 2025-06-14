using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoveRobotAssignment.Enums;
using MoveRobotAssignment.Feature.LeftCommand;
using MoveRobotAssignment.Feature.MoveCommand;
using MoveRobotAssignment.Feature.MoveToPositionCommand;
using MoveRobotAssignment.Feature.ReportQuery;
using MoveRobotAssignment.Feature.RightCommand;

namespace MoveRobotAssignment.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RobotMovesApi : ControllerBase
{
    private readonly IMediator _mediator;

    public RobotMovesApi(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("move")]
    public async Task<IActionResult> Move()
    {
        var result = await _mediator.Send(new MoveCommand());
        return Ok(result);
    }

    [HttpGet("report")]
    public async Task<IActionResult> Report()
    {
        var result = await _mediator.Send(new ReportQuery());
        return Ok(result);
    }

    [HttpPost("right")]
    public async Task<IActionResult> Right()
    {
        await _mediator.Send(new RightCommand());
        return NoContent();
    }

    [HttpPost("left")]
    public async Task<IActionResult> Left()
    {
        await _mediator.Send(new LeftCommand());
        return NoContent();
    }

    [HttpPost("moveToPosition")]
    public async Task<IActionResult> MoveToPosition(int x, int y, string direction)
    {
        direction = direction.ToUpperInvariant();
        if (!Enum.TryParse<Direction>(direction, out var parsedDirection))
        {
            return BadRequest($"Invalid direction: {direction}");
        }
        var result = await _mediator.Send(new MoveToPositionCommand(x, y, parsedDirection));
        return Ok(result);
    }
}
