using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.DirectApiCalls.Interfaces;

public interface IPrivateMessageCallService
{
  Task<Response<PrivateMessageComplex[]>> GetByUsers(int firstUser, int secondUser);
  Task<Response<PrivateMessageComplex[]>> GetUserConversations(int userId);
  Task<Response<PrivateMessageComplex>> SendMessage(PrivateMessageRequest request);
  Task<Response<PrivateMessageComplex>> UpdateMessage(PrivateMessageSimplified request);
  Task<Response<PrivateMessageComplex>> DeleteMessage(int messageId);
}
