using FluentValidation;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.Validation;

public class ChatChannelValidator : AbstractValidator<ChatChannelRequest>
{
  public ChatChannelValidator()
  {
    RuleFor(x => x.Name).NotEmpty();
  }
}
