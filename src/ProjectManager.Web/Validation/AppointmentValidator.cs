using FluentValidation;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.Validation;

public class AppointmentValidator : AbstractValidator<AppointmentRequest>
{
  public AppointmentValidator()
  {
    RuleFor(x => x.Name).NotEmpty();
    RuleFor(x => x.Description).NotEmpty();
    RuleFor(x => x.Date).NotEmpty().GreaterThanOrEqualTo(DateTime.UtcNow)
      .WithMessage("Appointment must be scheduled with today or future date.");
    RuleFor(x => x.Users).NotEmpty()
      .WithMessage("Appointment must be assigned to at least one user.");
  }
}
