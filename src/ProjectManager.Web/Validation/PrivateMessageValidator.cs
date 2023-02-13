using FluentValidation;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.Validation;

public class PrivateMessageValidator : AbstractValidator<PrivateMessageRequest>
{
  public PrivateMessageValidator()
  {
    RuleFor(x => x.Content).NotEmpty();
  }
}

public class UpdatePrivateMessageValidator : AbstractValidator<PrivateMessageSimplified>
{
  public UpdatePrivateMessageValidator()
  {
    RuleFor(x => x.Content).NotEmpty();
  }
}
