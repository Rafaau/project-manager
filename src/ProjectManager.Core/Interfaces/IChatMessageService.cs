using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManager.Core.ProjectAggregate;

namespace ProjectManager.Core.Interfaces;
public interface IChatMessageService
{
  Task<IQueryable<ChatMessage>> RetrieveAllMessages();
  Task<ChatMessage> PostMessage(ChatMessage request);
  Task<ChatMessage> EditMessage(int messageId, string content);
  Task<ChatMessage> DeleteMessage(int id);
}
