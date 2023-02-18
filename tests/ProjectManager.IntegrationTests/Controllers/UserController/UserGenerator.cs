using Bogus;
using ProjectManager.Core.ProjectAggregate.Enums;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.IntegrationTests.UserController;
public static class UserGenerator
{
  public static readonly Faker<UserSimplified> _userGenerator = new Faker<UserSimplified>()
    .RuleFor(x => x.Firstname, faker => ApiFactory.Username)
    .RuleFor(x => x.Lastname, faker => faker.Person.LastName)
    .RuleFor(x => x.Email, faker => faker.Person.Email)
    .RuleFor(x => x.Password, faker => faker.Internet.Password())
    .RuleFor(x => x.Role, faker => (UserRole)faker.Random.Number(0, 1));
}
