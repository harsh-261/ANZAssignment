using MediatR;
using MoveRobotAssignment.Enums;
using MoveRobotAssignment.Feature.PlaceCommand;

namespace MoveRobotAssignment.StateMachine;

public class RobotInitializer
{
    private readonly IMediator _mediator;
    private static bool _initialized = false;
    private static readonly object _lock = new();
    

    public RobotInitializer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task InitAsync()
    {
        if (_initialized) return;

        lock (_lock)
        {
            if (_initialized) return;
            _initialized = true;
        }

        await _mediator.Send(new PlaceCommand(0, 0, Direction.NORTH));
    }
}
