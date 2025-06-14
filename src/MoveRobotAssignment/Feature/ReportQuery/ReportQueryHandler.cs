using MediatR;
using MoveRobotAssignment.StateMachine;
using MoveRobotAssignment.StateMachine.Interface;

namespace MoveRobotAssignment.Feature.ReportQuery;

public class ReportQueryHandler : IRequestHandler<ReportQuery, string>
{
    private readonly IBoard _board;

    public ReportQueryHandler(IBoard board) => _board = board;

    public Task<string> Handle(ReportQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_board.Report());
    }
}

