using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using ProjectManager.Web.DirectApiCalls.Interfaces;

namespace ProjectManager.Web.DirectApiCalls.Services;

public class PrivateMessageCallService : ServiceBase, IPrivateMessageCallService
{
  public PrivateMessageCallService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
  {
  }

  public async Task<Response<PrivateMessageComplex[]>> GetByUsers(int firstUser, int secondUser)
  {
    return await HttpClient.GetResponse<PrivateMessageComplex[]>($"/api/privatemessage?$filter=(senderId eq {firstUser} and receiverId eq {secondUser}) or (senderId eq {secondUser} and receiverId eq {firstUser})");
  }

  public async Task<Response<PrivateMessageComplex[]>> GetUserConversations(int userId)
  {
    return await HttpClient.GetResponse<PrivateMessageComplex[]>($"/api/privatemessage/{userId}");
  }

  public async Task<Response<PrivateMessageComplex>> SendMessage(PrivateMessageRequest request)
  {
    return await HttpClient.Post<PrivateMessageRequest, PrivateMessageComplex>("/api/privatemessage", request);
  }

  public async Task<Response<PrivateMessageComplex>> UpdateMessage(PrivateMessageSimplified request)
  {
    return await HttpClient.Put<PrivateMessageSimplified, PrivateMessageComplex>("/api/privatemessage", request);
  }

  public async Task<Response<PrivateMessageComplex>> DeleteMessage(int messageId)
  {
    return await HttpClient.Delete<PrivateMessageComplex>($"/api/privatemessage/{messageId}");
  }
}
