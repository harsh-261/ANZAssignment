using MediatR;
using MoveRobotAssignment.StateMachine;
using MoveRobotAssignment.StateMachine.Interface;

namespace MoveRobotAssignment.Feature.PlaceCommand;

public class PlaceCommandHandler : IRequestHandler<PlaceCommand, bool>
{
    private readonly IBoard _board;

    public PlaceCommandHandler(IBoard board) => _board = board;

    public Task<bool> Handle(PlaceCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_board.Place(request.X, request.Y, request.Facing));
    }
}
