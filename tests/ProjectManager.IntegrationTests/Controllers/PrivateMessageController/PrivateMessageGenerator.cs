using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.IntegrationTests.PrivateMessageController;
public class PrivateMessageGenerator
{
  public static readonly Faker<PrivateMessageRequest> _privateMessageGenerator = new Faker<PrivateMessageRequest>()
    .RuleFor(x => x.Content, faker => faker.Lorem.Sentence(5, 10));
}
