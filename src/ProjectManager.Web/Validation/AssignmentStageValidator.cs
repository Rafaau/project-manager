using FluentValidation;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.Validation;

public class AssignmentStageValidator : AbstractValidator<AssignmentStageRequest>
{
  public AssignmentStageValidator()
  {
    RuleFor(x => x.ProjectId).NotEmpty();
    RuleFor(x => x.Index).NotEmpty();
  }
}
