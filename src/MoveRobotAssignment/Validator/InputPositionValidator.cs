using FluentValidation;
using MoveRobotAssignment.Models;
using MoveRobotAssignment.Enums;

namespace MoveRobotAssignment.Validator;

public class InputPositionValidator : AbstractValidator<InputPosition>
{
    public InputPositionValidator()
    {
        RuleFor(x => x.X).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Y).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Direction)
            .NotEmpty()
            .Must(d => Enum.TryParse<Direction>(d?.ToUpperInvariant(), true, out _))
            .WithMessage("Invalid direction value.");
    }
}

