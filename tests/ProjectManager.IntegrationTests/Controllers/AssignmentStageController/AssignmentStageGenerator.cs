using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.IntegrationTests.AssignmentStageController;
public static class AssignmentStageGenerator
{
  public static readonly Faker<AssignmentStageRequest> _stageGenerator = new Faker<AssignmentStageRequest>()
    .RuleFor(x => x.Index, faker => faker.Random.Number(4, 6));
}
