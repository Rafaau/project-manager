using Bogus;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.IntegrationTests.ProjectController;
public static class ProjectGenerator
{
  public static readonly Faker<ProjectRequest> _projectGenerator = new Faker<ProjectRequest>()
    .RuleFor(x => x.Name, faker => faker.Company.CompanyName());
}
