using MoveRobotAssignment.Enums;

namespace MoveRobotAssignment.Models;

public class Position
{
    public int X { get; set; }
    public int Y { get; set; }
    public Direction Facing { get; set; }

    public override string ToString() => $"({X}, {Y}) Facing: {Facing}";
}