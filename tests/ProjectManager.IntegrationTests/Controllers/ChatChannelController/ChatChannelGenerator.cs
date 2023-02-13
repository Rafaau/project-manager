﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.IntegrationTests.ChatChannelController;
public class ChatChannelGenerator
{
  public static readonly Faker<ChatChannelRequest> _chatChannelGenerator = new Faker<ChatChannelRequest>()
    .RuleFor(x => x.Name, faker => faker.Lorem.Sentence(2, 2));
}
