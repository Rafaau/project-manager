using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using ProjectManager.Web.DirectApiCalls.Interfaces;

namespace ProjectManager.Web.DirectApiCalls.Services;

public class ChatChannelCallService : ServiceBase, IChatChannelCallService
{
  public ChatChannelCallService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
  {
  }

  public async Task<Response<ChatChannelComplex>> CreateChatChannel(ChatChannelRequest request)
  {
    return await HttpClient.Post<ChatChannelRequest, ChatChannelComplex>("/api/chatchannel", request);
  }

  public async Task<Response<ChatChannelComplex>> UpdateChatChannel(ChatChannelSimplified request)
  {
    return await HttpClient.Put<ChatChannelSimplified, ChatChannelComplex>("/api/chatchannel", request);
  }

  public async Task<Response<ChatChannelComplex>> DeleteChatChannel(int channelId)
  {
    return await HttpClient.Delete<ChatChannelComplex>($"/api/chatchannel/{channelId}");
  }
}
