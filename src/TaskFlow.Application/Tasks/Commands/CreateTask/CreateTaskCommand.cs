using AutoMapper;
using FluentValidation;
using MediatR;
using TaskFlow.Application.Common.Interfaces;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Application.Tasks.Commands.CreateTask;

public record CreateTaskCommand(
    string Title,
    string? Description,
    TaskPriority Priority,
    DateTime? DueDate,
    Guid ProjectId,
    Guid? AssignedToUserId) : IRequest<TaskDto>;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("O título é obrigatório.")
            .MaximumLength(150).WithMessage("O título deve ter no máximo 150 caracteres.");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("A descrição deve ter no máximo 2000 caracteres.");

        RuleFor(x => x.ProjectId)
            .NotEmpty().WithMessage("O projeto é obrigatório.");

        RuleFor(x => x.Priority)
            .IsInEnum();
    }
}

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateTaskCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var projectExists = await _context.Projects
            .FindAsync(new object[] { request.ProjectId }, cancellationToken);

        if (projectExists is null)
        {
            throw new NotFoundException(nameof(ProjectEntity), request.ProjectId);
        }

        var entity = new Domain.Entities.TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            Priority = request.Priority,
            DueDate = request.DueDate,
            ProjectId = request.ProjectId,
            AssignedToUserId = request.AssignedToUserId,
            Status = TaskItemStatus.Todo
        };

        _context.Tasks.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TaskDto>(entity);
    }
}
