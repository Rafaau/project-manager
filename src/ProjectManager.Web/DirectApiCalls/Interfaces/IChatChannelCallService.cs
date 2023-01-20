using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.DirectApiCalls.Interfaces;

public interface IChatChannelCallService
{
  Task<Response<ChatChannelComplex>> CreateChatChannel(ChatChannelRequest request);
  Task<Response<ChatChannelComplex>> DeleteChatChannel(int channelId);
  Task<Response<ChatChannelComplex>> UpdateChatChannel(ChatChannelSimplified request);
}
