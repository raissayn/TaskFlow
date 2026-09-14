using FluentAssertions;
using TaskFlow.Application.Tasks.Commands.CreateTask;
using TaskFlow.Domain.Enums;
using Xunit;

namespace TaskFlow.Application.Tests.Tasks;

public class CreateTaskCommandValidatorTests
{
    private readonly CreateTaskCommandValidator _validator = new();

    [Fact]
    public void Should_Fail_When_Title_Is_Empty()
    {
        var command = new CreateTaskCommand(
            Title: "",
            Description: "desc",
            Priority: TaskPriority.Medium,
            DueDate: null,
            ProjectId: Guid.NewGuid(),
            AssignedToUserId: null);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateTaskCommand.Title));
    }

    [Fact]
    public void Should_Fail_When_ProjectId_Is_Empty()
    {
        var command = new CreateTaskCommand(
            Title: "Valid title",
            Description: null,
            Priority: TaskPriority.Low,
            DueDate: null,
            ProjectId: Guid.Empty,
            AssignedToUserId: null);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateTaskCommand.ProjectId));
    }

    [Fact]
    public void Should_Pass_When_Command_Is_Valid()
    {
        var command = new CreateTaskCommand(
            Title: "Implement login screen",
            Description: "Build the login screen with validation",
            Priority: TaskPriority.High,
            DueDate: DateTime.UtcNow.AddDays(3),
            ProjectId: Guid.NewGuid(),
            AssignedToUserId: null);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }
}
