using Bogus;
using ProjectManager.Core.ProjectAggregate.Enums;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.IntegrationTests;
public static class FakerGenerator
{
  public static readonly Faker<AppointmentRequest> _appointmentGenerator = new Faker<AppointmentRequest>()
    .RuleFor(x => x.Name, faker => faker.Lorem.Sentence(2, 2))
    .RuleFor(x => x.Description, faker => faker.Lorem.Sentence(5, 0))
    .RuleFor(x => x.Date, faker => DateTime.UtcNow.AddDays(2));

  public static readonly Faker<AssignmentRequest> _assignmentGenerator = new Faker<AssignmentRequest>()
    .RuleFor(x => x.Name, faker => faker.Lorem.Sentence(2, 2))
    .RuleFor(x => x.Description, faker => faker.Lorem.Sentence(5, 5))
    .RuleFor(x => x.Deadline, faker => DateTime.UtcNow.AddDays(12))
    .RuleFor(x => x.Priority, faker => (Priority)faker.Random.Number(0, 2));

  public static readonly Faker<AssignmentStageRequest> _stageGenerator = new Faker<AssignmentStageRequest>()
    .RuleFor(x => x.Index, faker => faker.Random.Number(4, 6));

  public static readonly Faker<ChatChannelRequest> _chatChannelGenerator = new Faker<ChatChannelRequest>()
    .RuleFor(x => x.Name, faker => faker.Lorem.Sentence(2, 2));

  public static readonly Faker<ChatMessageRequest> _chatMessageGenerator = new Faker<ChatMessageRequest>()
    .RuleFor(x => x.Content, faker => faker.Lorem.Sentence(5, 10));

  public static readonly Faker<NotificationRequest> _notificationGenerator = new Faker<NotificationRequest>()
    .RuleFor(x => x.Content, faker => faker.Lorem.Sentence(8, 5));

  public static readonly Faker<PrivateMessageRequest> _privateMessageGenerator = new Faker<PrivateMessageRequest>()
    .RuleFor(x => x.Content, faker => faker.Lorem.Sentence(5, 10));

  public static readonly Faker<ProjectRequest> _projectGenerator = new Faker<ProjectRequest>()
    .RuleFor(x => x.Name, faker => faker.Company.CompanyName());

  public static readonly Faker<UserSimplified> _userGenerator = new Faker<UserSimplified>()
    .RuleFor(x => x.Firstname, faker => ApiFactory.Username)
    .RuleFor(x => x.Lastname, faker => faker.Person.LastName)
    .RuleFor(x => x.Email, faker => faker.Person.Email)
    .RuleFor(x => x.Password, faker => faker.Internet.Password())
    .RuleFor(x => x.Role, faker => (UserRole)faker.Random.Number(0, 1));
}
