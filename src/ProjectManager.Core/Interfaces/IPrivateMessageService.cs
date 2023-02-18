using ProjectManager.Core.ProjectAggregate;

namespace ProjectManager.Core.Interfaces;
public interface IPrivateMessageService
{
  Task<IQueryable<PrivateMessage>> RetrieveAllMessages();
  Task<PrivateMessage[]> GetUserConversations(int userId);
  Task<PrivateMessage> PostMessage(PrivateMessage request);
  Task<PrivateMessage> SetMessageAsSeen(int messageId);
  Task<PrivateMessage> EditMessage(PrivateMessage request);
  Task<PrivateMessage> DeleteMessage(int messageId);
}
