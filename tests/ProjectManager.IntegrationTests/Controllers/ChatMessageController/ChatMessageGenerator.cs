using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using ProjectManager.Web.ApiModels;
using Xunit;

namespace ProjectManager.IntegrationTests.ChatMessageController;

public class ChatMessageGenerator
{
  public static readonly Faker<ChatMessageRequest> _chatMessageGenerator = new Faker<ChatMessageRequest>()
    .RuleFor(x => x.Content, faker => faker.Lorem.Sentence(5, 10));
}
