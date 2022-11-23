using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.Validation;

public class UserValidator : AbstractValidator<UserRequest>
{
  public UserValidator()
  {
    RuleFor(x => x.Firstname).NotEmpty();
    RuleFor(x => x.Email).NotEmpty()
      .EmailAddress()
      .Matches(EmailRegex)
      .WithMessage(x => $"{x.Email} is not a valid email address.");
    RuleFor(x => x.Password).NotEmpty()
      .MinimumLength(6).WithMessage("Password must have at least 6 letters.")
      /*.Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.")
      .Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter.")
      .Matches(@"[0-9]+").WithMessage("Password must containt at least one number.")
      .Matches(@"[\!\?\*\.]+").WithMessage("Password must contain at least one special char.")*/;
  }

  private static readonly Regex EmailRegex =
    new("^[\\w!#$%&’*+/=?`{|}~^-]+(?:\\.[\\w!#$%&’*+/=?`{|}~^-]+)*@(?:[a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$",
      RegexOptions.Compiled | RegexOptions.IgnoreCase);
}

public class UserUpdateValidator : AbstractValidator<UserSimplified>
{
  public UserUpdateValidator()
  {
    RuleFor(x => x.Firstname).NotEmpty();
    RuleFor(x => x.Email).NotEmpty()
      .EmailAddress()
      .Matches(EmailRegex)
      .WithMessage(x => $"{x.Email} is not a valid email address.");
    RuleFor(x => x.Password).NotEmpty()
      .MinimumLength(6).WithMessage("Password must have at least 6 letters.")
      /*.Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.")
      .Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter.")
      .Matches(@"[0-9]+").WithMessage("Password must containt at least one number.")
      .Matches(@"[\!\?\*\.]+").WithMessage("Password must contain at least one special char.")*/;
  }

  private static readonly Regex EmailRegex =
    new("^[\\w!#$%&’*+/=?`{|}~^-]+(?:\\.[\\w!#$%&’*+/=?`{|}~^-]+)*@(?:[a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$",
      RegexOptions.Compiled | RegexOptions.IgnoreCase);
}
