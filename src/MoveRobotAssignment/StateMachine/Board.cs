using Serilog;
using Microsoft.Extensions.Caching.Memory;
using MoveRobotAssignment.Enums;
using MoveRobotAssignment.Models;
using MoveRobotAssignment.StateMachine.Interface;

namespace MoveRobotAssignment.StateMachine;

public class Board : IBoard
{
    private const int Size = 5;
    private readonly IMemoryCache _cache;
    private readonly ILogger<Board> _logger;
    private const string CacheKey = "robot-position";

    public Board(IMemoryCache cache, ILogger<Board> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    private Position? Position
    {
        get => _cache.TryGetValue(CacheKey, out Position position) ? position : null;
        set
        {
            if (value == null)
                _cache.Remove(CacheKey);
            else
                _cache.Set(CacheKey, value);
        }
    }

    public bool Place(int x, int y, Direction direction)
    {
        if (!IsValid(x, y)) return false;

        Position = new Position { X = x, Y = y, Facing = direction };
        _logger.LogInformation($"Placed at {x},{y},{direction}");
        return true;
    }

    public bool Move()
    {
        var pos = Position;
        if (pos == null) return false;

        int x = pos.X, y = pos.Y;
        switch (pos.Facing)
        {
            case Direction.NORTH: y++; break;
            case Direction.EAST: x++; break;
            case Direction.SOUTH: y--; break;
            case Direction.WEST: x--; break;
        }

        if (!IsValid(x, y)) return false;

        pos.X = x;
        pos.Y = y;
        Position = pos;

        _logger.LogInformation($"Moved to {x},{y}");
        return true;
    }

    public void TurnLeft()
    {
        var pos = Position;
        if (pos == null) return;

        pos.Facing = (Direction)(((int)pos.Facing + 3) % 4);
        Position = pos;

        _logger.LogInformation($"Turned Left. Now facing {pos.Facing}");
    }

    public void TurnRight()
    {
        var pos = Position;
        if (pos == null) return;

        pos.Facing = (Direction)(((int)pos.Facing + 1) % 4);
        Position = pos;

        _logger.LogInformation($"Turned Right. Now facing {pos.Facing}");
    }

    public string Report()
    {
        var pos = Position;
        if (pos == null)
        {
            _logger.LogWarning("Not placed yet");
            return "Not placed";
        }

        return $"{pos.X},{pos.Y},{pos.Facing}";
    }

    private bool IsValid(int x, int y) => x >= 0 && x <= Size && y >= 0 && y <= Size;
}
