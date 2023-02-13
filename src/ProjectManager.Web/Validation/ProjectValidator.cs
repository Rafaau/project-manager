using FluentValidation;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.Validation;

public class ProjectValidator : AbstractValidator<ProjectRequest>
{
  public ProjectValidator()
  {
    RuleFor(x => x.Name).NotEmpty();
  }
}

public class UpdateProjectValidator : AbstractValidator<ProjectComplex>
{
  public UpdateProjectValidator()
  {
    RuleFor(x => x.Name).NotEmpty();
  }
}
