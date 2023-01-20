using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManager.Core.ProjectAggregate;

namespace ProjectManager.Core.Interfaces;
public interface IChatChannelService
{
  Task<ChatChannel> CreateChatChannel(ChatChannel request);
  Task<ChatChannel> DeleteChatChannel(int channelId);
  Task<ChatChannel> UpdateChatChannel(ChatChannel request);
}
