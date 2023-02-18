using Bogus;
using ProjectManager.Core.ProjectAggregate.Enums;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.IntegrationTests.AssignmentController;

public static class AssignmentGenerator
{
  public static readonly Faker<AssignmentRequest> _assignmentGenerator = new Faker<AssignmentRequest>()
    .RuleFor(x => x.Name, faker => faker.Lorem.Sentence(2, 2))
    .RuleFor(x => x.Description, faker => faker.Lorem.Sentence(5,5))
    .RuleFor(x => x.Deadline, faker => DateTime.UtcNow.AddDays(12))
    .RuleFor(x => x.Priority, faker => (Priority)faker.Random.Number(0,2));
}
