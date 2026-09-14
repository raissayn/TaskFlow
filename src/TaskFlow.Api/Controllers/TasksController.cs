using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Tasks.Commands.CompleteTask;
using TaskFlow.Application.Tasks.Commands.CreateTask;
using TaskFlow.Application.Tasks.Commands.DeleteTask;
using TaskFlow.Application.Tasks.Commands.UpdateTask;
using TaskFlow.Application.Tasks.Queries.GetTaskById;
using TaskFlow.Application.Tasks.Queries.GetTasks;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Lista tarefas, com filtros opcionais por projeto, status e prioridade.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<TaskDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<TaskDto>>> GetAll(
        [FromQuery] Guid? projectId,
        [FromQuery] TaskItemStatus? status,
        [FromQuery] TaskPriority? priority)
    {
        var result = await _mediator.Send(new GetTasksQuery(projectId, status, priority));
        return Ok(result);
    }

    /// <summary>Obtém uma tarefa pelo Id.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetTaskByIdQuery(id));
        return Ok(result);
    }

    /// <summary>Cria uma nova tarefa dentro de um projeto.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<TaskDto>> Create(CreateTaskCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Atualiza uma tarefa existente.</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<TaskDto>> Update(Guid id, UpdateTaskCommand command)
    {
        if (id != command.Id) return BadRequest("O Id da rota não corresponde ao Id do corpo da requisição.");

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>Marca uma tarefa como concluída.</summary>
    [HttpPatch("{id:guid}/complete")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<TaskDto>> Complete(Guid id)
    {
        var result = await _mediator.Send(new CompleteTaskCommand(id));
        return Ok(result);
    }

    /// <summary>Remove uma tarefa.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteTaskCommand(id));
        return NoContent();
    }
}
