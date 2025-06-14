using MediatR;

namespace MoveRobotAssignment.Feature.ReportQuery;

public record ReportQuery() : IRequest<string>;
