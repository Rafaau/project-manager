using Bogus;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.IntegrationTests.NotificationController;
public class NotificationGenerator
{
  public static readonly Faker<NotificationRequest> _notificationGenerator = new Faker<NotificationRequest>()
    .RuleFor(x => x.Content, faker => faker.Lorem.Sentence(8,5));
}
