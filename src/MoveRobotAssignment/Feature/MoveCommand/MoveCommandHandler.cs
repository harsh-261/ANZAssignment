using MediatR;
using MoveRobotAssignment.StateMachine;
using MoveRobotAssignment.StateMachine.Interface;

namespace MoveRobotAssignment.Feature.MoveCommand;

public class MoveCommandHandler : IRequestHandler<MoveCommand, bool>
{
    private readonly IBoard _board;

    public MoveCommandHandler(IBoard board) => _board = board;

    public Task<bool> Handle(MoveCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_board.Move());
    }
}
