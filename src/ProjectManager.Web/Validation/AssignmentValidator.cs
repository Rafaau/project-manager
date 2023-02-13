using FluentValidation;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.Validation;

public class AssignmentValidator : AbstractValidator<AssignmentRequest>
{
  public AssignmentValidator()
  {
    RuleFor(x => x.Name).NotEmpty();
    RuleFor(x => x.Description).NotEmpty();
    RuleFor(x => x.AssignmentStageId).NotEmpty();
    RuleFor(x => x.ProjectId).NotEmpty();
    RuleFor(x => x.Priority).NotNull();
    RuleFor(x => x.Deadline)
      .NotEmpty()
      .GreaterThan(DateTime.Now.Date)
      .WithMessage("Deadline should be set as later than today.");
  }
}

public class UpdateAssignmentValidator : AbstractValidator<AssignmentComplex>
{
  public UpdateAssignmentValidator()
  {
    RuleFor(x => x.Name).NotEmpty();
    RuleFor(x => x.Description).NotEmpty();
    RuleFor(x => x.Project).NotNull();
    RuleFor(x => x.Priority).NotNull();
    RuleFor(x => x.Deadline)
      .NotEmpty()
      .GreaterThan(DateTime.Now.Date)
      .WithMessage("Deadline should be set as later than today.");
  }
}
