using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using MoveRobotAssignment.Enums;
using MoveRobotAssignment.Models;
using MoveRobotAssignment.StateMachine;

namespace MoveRobotAssignmentTests.StateMachineTests;

public class BoardTests
{
    private readonly Mock<IMemoryCache> _cacheMock;
    private readonly Mock<ILogger<Board>> _loggerMock;
    private readonly Mock<ICacheEntry> _cacheEntryMock;
    private readonly Board _board;
    private Position _position;

    public BoardTests()
    {
        _cacheMock = new Mock<IMemoryCache>();
        _loggerMock = new Mock<ILogger<Board>>();
        _cacheEntryMock = new Mock<ICacheEntry>();

        // Setup TryGetValue to return false
        object dummy;
        _cacheMock.Setup(c => c.TryGetValue(It.IsAny<object>(), out dummy)).Returns(false);

        // Setup CreateEntry to track any calls (if valid placement)
        _cacheMock.Setup(c => c.CreateEntry(It.IsAny<object>())).Returns(_cacheEntryMock.Object);

        _board = new Board(_cacheMock.Object, _loggerMock.Object);
    }

    private void SetupCache(Position pos)
    {
        _cacheMock.Setup(c => c.TryGetValue(It.IsAny<object>(), out pos))
            .Returns(true);
    }

    [Fact]
    public void Place_ValidPosition_ReturnsTrueAndSetsPosition()
    {
        // Arrange
        var cacheEntryMock = new Mock<ICacheEntry>();
        _cacheMock.Setup(c => c.CreateEntry(It.IsAny<object>())).Returns(cacheEntryMock.Object);

        // Act
        var result = _board.Place(1, 2, Direction.NORTH);

        // Assert
        Assert.True(result);

        _cacheMock.Verify(c => c.CreateEntry(It.IsAny<object>()), Times.Once);
        cacheEntryMock.VerifySet(entry => entry.Value = It.Is<Position>(p =>
            p.X == 1 && p.Y == 2 && p.Facing == Direction.NORTH), Times.Once);
    }

    [Fact]
    public void Place_InvalidPosition_ReturnsFalse()
    {
        // Act
        var result = _board.Place(-1, 10, Direction.EAST);

        // Assert
        Assert.False(result);
        _cacheMock.Verify(c => c.CreateEntry(It.IsAny<object>()), Times.Never);
    }

    [Fact]
    public void Move_ValidMove_UpdatesPositionAndReturnsTrue()
    {
        // Arrange
        var position = new Position { X = 1, Y = 1, Facing = Direction.NORTH };

        // Mock TryGetValue to return initial position
        object outObj;
        _cacheMock.Setup(c => c.TryGetValue(It.IsAny<object>(), out outObj))
                  .Callback(new TryGetValueCallback((object key, out object value) => value = position))
                  .Returns(true);

        // Mock CreateEntry
        var cacheEntryMock = new Mock<ICacheEntry>();
        _cacheMock.Setup(c => c.CreateEntry(It.IsAny<object>())).Returns(cacheEntryMock.Object);

        // Act
        var result = _board.Move();

        // Assert
        Assert.True(result);
        _cacheMock.Verify(c => c.CreateEntry(It.IsAny<object>()), Times.Once);
        cacheEntryMock.VerifySet(e => e.Value = It.Is<Position>(p => p.X == 1 && p.Y == 2 && p.Facing == Direction.NORTH), Times.Once);
    }


    [Fact]
    public void Move_InvalidMove_ReturnsFalse()
    {
        // Arrange
        var position = new Position { X = 5, Y = 5, Facing = Direction.NORTH };

        object outObj;
        _cacheMock.Setup(c => c.TryGetValue(It.IsAny<object>(), out outObj))
                  .Callback(new TryGetValueCallback((object key, out object value) => value = position))
                  .Returns(true);

        // Act
        var result = _board.Move();

        // Assert
        Assert.False(result);
        _cacheMock.Verify(c => c.CreateEntry(It.IsAny<object>()), Times.Never);
    }



    [Fact]
    public void Move_NotPlaced_ReturnsFalse()
    {
        // Arrange
        object outObj = null;
        _cacheMock.Setup(c => c.TryGetValue(It.IsAny<object>(), out outObj)).Returns(false);

        // Act
        var result = _board.Move();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TurnLeft_UpdatesFacing()
    {
        // Arrange
        var position = new Position { X = 1, Y = 1, Facing = Direction.NORTH };

        object outObj;
        _cacheMock.Setup(c => c.TryGetValue(It.IsAny<object>(), out outObj))
                  .Callback(new TryGetValueCallback((object key, out object value) => value = position))
                  .Returns(true);

        var cacheEntryMock = new Mock<ICacheEntry>();
        _cacheMock.Setup(c => c.CreateEntry(It.IsAny<object>())).Returns(cacheEntryMock.Object);

        // Act
        _board.TurnLeft();

        // Assert
        _cacheMock.Verify(c => c.CreateEntry(It.IsAny<object>()), Times.Once);
        cacheEntryMock.VerifySet(e => e.Value = It.Is<Position>(p => p.Facing == Direction.WEST), Times.Once);
    }


    [Fact]
    public void TurnRight_UpdatesFacing()
    {
        // Arrange
        var position = new Position { X = 1, Y = 1, Facing = Direction.NORTH };

        object outObj;
        _cacheMock.Setup(c => c.TryGetValue(It.IsAny<object>(), out outObj))
                  .Callback(new TryGetValueCallback((object key, out object value) => value = position))
                  .Returns(true);

        var cacheEntryMock = new Mock<ICacheEntry>();
        _cacheMock.Setup(c => c.CreateEntry(It.IsAny<object>())).Returns(cacheEntryMock.Object);

        // Act
        _board.TurnRight();

        // Assert
        _cacheMock.Verify(c => c.CreateEntry(It.IsAny<object>()), Times.Once);
        cacheEntryMock.VerifySet(e => e.Value = It.Is<Position>(p => p.Facing == Direction.EAST), Times.Once);
    }


    [Fact]
    public void Report_WhenPlaced_ReturnsPositionString()
    {
        // Arrange
        var pos = new Position { X = 2, Y = 3, Facing = Direction.SOUTH };
        object outObj = pos;
        _cacheMock.Setup(c => c.TryGetValue(It.IsAny<object>(), out outObj)).Returns(true);

        // Act
        var result = _board.Report();

        // Assert
        Assert.Equal("2,3,SOUTH", result);
    }

    [Fact]
    public void Report_NotPlaced_ReturnsNotPlaced()
    {
        // Arrange
        object outObj = null;
        _cacheMock.Setup(c => c.TryGetValue(It.IsAny<object>(), out outObj)).Returns(false);

        // Act
        var result = _board.Report();

        // Assert
        Assert.Equal("Not placed", result);
    }

    private delegate void TryGetValueCallback(object key, out object value);
}

