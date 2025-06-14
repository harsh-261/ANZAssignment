using MoveRobotAssignment.Enums;

namespace MoveRobotAssignment.StateMachine.Interface;

public interface IBoard
{
    void TurnLeft();

    void TurnRight();

    string Report();  

    bool Move();

    bool Place(int x, int y, Direction direction);
}