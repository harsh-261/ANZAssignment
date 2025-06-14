using MediatR;
using MoveRobotAssignment.Enums;

namespace MoveRobotAssignment.Feature.PlaceCommand;

public record PlaceCommand(int X, int Y, Direction Facing) : IRequest<bool>;
