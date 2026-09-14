using AutoMapper;
using FluentValidation;
using MediatR;
using TaskFlow.Application.Common.Interfaces;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Application.Projects.Commands.CreateProject;

public record CreateProjectCommand(string Name, string? Description) : IRequest<ProjectDto>;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome do projeto é obrigatório.")
            .MaximumLength(100);
    }
}

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public CreateProjectCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _context = context;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is null)
        {
            throw new ForbiddenAccessException("Usuário não autenticado.");
        }

        var entity = new ProjectEntity
        {
            Name = request.Name,
            Description = request.Description,
            OwnerId = _currentUserService.UserId.Value
        };

        _context.Projects.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ProjectDto>(entity);
    }
}
