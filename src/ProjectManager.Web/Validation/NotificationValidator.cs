using FluentValidation;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.Validation;

public class NotificationValidator : AbstractValidator<NotificationRequest>
{
  public NotificationValidator()
  {
    RuleFor(x => x.Content).NotEmpty();
  }
}
