using MediatR;
using MoveRobotAssignment.StateMachine;
using MoveRobotAssignment.StateMachine.Interface;

namespace MoveRobotAssignment.Feature.MoveToPositionCommand;

public class MoveToPositionCommandHandler : IRequestHandler<MoveToPositionCommand, bool>
{
    private readonly IBoard _board;

    public MoveToPositionCommandHandler(IBoard board) => _board = board;

    public Task<bool> Handle(MoveToPositionCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_board.Place(request.x, request.y, request.direction));

    }
}
