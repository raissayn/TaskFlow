namespace TaskFlow.Domain.Entities;

public class ProjectEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid OwnerId { get; set; }

    public User? Owner { get; set; }
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
