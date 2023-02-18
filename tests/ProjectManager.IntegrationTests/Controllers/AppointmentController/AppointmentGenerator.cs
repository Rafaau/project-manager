using Bogus;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.IntegrationTests.AppointmentController;
public static class AppointmentGenerator
{
  public static readonly Faker<AppointmentRequest> _appointmentGenerator = new Faker<AppointmentRequest>()
    .RuleFor(x => x.Name, faker => faker.Lorem.Sentence(2, 2))
    .RuleFor(x => x.Description, faker => faker.Lorem.Sentence(5, 0))
    .RuleFor(x => x.Date, faker => DateTime.UtcNow.AddDays(2));
}
