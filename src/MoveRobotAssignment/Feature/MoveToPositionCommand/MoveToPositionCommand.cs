using MediatR;
using MoveRobotAssignment.Enums;

namespace MoveRobotAssignment.Feature.MoveToPositionCommand;

public record MoveToPositionCommand(int x, int y, Direction direction) : IRequest<bool>;
