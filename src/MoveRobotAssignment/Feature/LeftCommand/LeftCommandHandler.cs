using MediatR;
using MoveRobotAssignment.StateMachine.Interface;

namespace MoveRobotAssignment.Feature.LeftCommand;

public class LeftCommandHandler : IRequestHandler<LeftCommand>
{
    private readonly IBoard _board;

    public LeftCommandHandler(IBoard board) => _board = board;

    Task<Unit> IRequestHandler<LeftCommand, Unit>.Handle(LeftCommand request, CancellationToken cancellationToken)
    {
        _board.TurnLeft();
        return Task.FromResult(Unit.Value);
    }
}
