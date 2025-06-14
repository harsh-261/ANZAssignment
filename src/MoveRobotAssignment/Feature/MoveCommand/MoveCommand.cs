using MediatR;

namespace MoveRobotAssignment.Feature.MoveCommand;

public record MoveCommand() : IRequest<bool>;
