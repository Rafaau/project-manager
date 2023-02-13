using FluentValidation;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.Validation;

public class ChatMessageValidator : AbstractValidator<ChatMessageRequest>
{
  public ChatMessageValidator()
  {
    RuleFor(x => x.Content).NotEmpty();
  }
}
