using MediatR;
using MoveRobotAssignment.StateMachine;
using MoveRobotAssignment.StateMachine.Interface;

namespace MoveRobotAssignment.Feature.RightCommand;

public class RightCommandHandler : IRequestHandler<RightCommand>
{
    private readonly IBoard _board;

    public RightCommandHandler(IBoard board) => _board = board;

    Task<Unit> IRequestHandler<RightCommand, Unit>.Handle(RightCommand request, CancellationToken cancellationToken)
    {
        _board.TurnRight();
        return Task.FromResult(Unit.Value);
    }
}
