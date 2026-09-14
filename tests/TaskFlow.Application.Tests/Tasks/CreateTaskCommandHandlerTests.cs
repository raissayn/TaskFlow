using AutoMapper;
using FluentAssertions;
using TaskFlow.Application.Common.Mappings;
using TaskFlow.Application.Tasks.Commands.CreateTask;
using TaskFlow.Application.Tests.Common;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;
using TaskFlow.Domain.Exceptions;
using Xunit;

namespace TaskFlow.Application.Tests.Tasks;

public class CreateTaskCommandHandlerTests
{
    private readonly IMapper _mapper;

    public CreateTaskCommandHandlerTests()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = configuration.CreateMapper();
    }

    [Fact]
    public async Task Should_Create_Task_When_Project_Exists()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create();

        var project = new ProjectEntity { Name = "Website Redesign", OwnerId = Guid.NewGuid() };
        context.Projects.Add(project);
        await context.SaveChangesAsync(CancellationToken.None);

        var handler = new CreateTaskCommandHandler(context, _mapper);
        var command = new CreateTaskCommand(
            Title: "Design homepage wireframe",
            Description: "Low fidelity wireframe for the new homepage",
            Priority: TaskPriority.High,
            DueDate: DateTime.UtcNow.AddDays(5),
            ProjectId: project.Id,
            AssignedToUserId: null);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be(command.Title);
        result.Status.Should().Be(TaskItemStatus.Todo);
        context.Tasks.Should().ContainSingle(t => t.Id == result.Id);
    }

    [Fact]
    public async Task Should_Throw_NotFoundException_When_Project_Does_Not_Exist()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create();
        var handler = new CreateTaskCommandHandler(context, _mapper);

        var command = new CreateTaskCommand(
            Title: "Task without a project",
            Description: null,
            Priority: TaskPriority.Low,
            DueDate: null,
            ProjectId: Guid.NewGuid(),
            AssignedToUserId: null);

        // Act
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
