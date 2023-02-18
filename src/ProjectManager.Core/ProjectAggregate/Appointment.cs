using ProjectManager.SharedKernel;
using ProjectManager.SharedKernel.Interfaces;

namespace ProjectManager.Core.ProjectAggregate;
public class Appointment : EntityBase, IAggregateRoot
{
  public string Name { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public DateTime Date { get; set; }
  public virtual ICollection<User> Users { get; set; }

  public Appointment()
  {
  }
}
