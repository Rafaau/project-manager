using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Core.Services;
public class ChatMessageCallService : ServiceBase, IChatMessageCallService
{
  public ChatMessageCallService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
  {
  }
  public async Task<Response<ChatMessageComplex[]>> GetAll()
  {
    return await HttpClient.GetResponse<ChatMessageComplex[]>("/api/chatmessage");
  }

  public async Task<Response<ChatMessageComplex[]>> GetByProjectId(int projectId)
  {
    return await HttpClient.GetResponse<ChatMessageComplex[]>($"/api/chatmessage?$filter=projectId eq {projectId}");
  }

  public async Task<Response<ChatMessageComplex>> AddMessage(ChatMessageRequest message)
  {
    return await HttpClient.Post<ChatMessageRequest, ChatMessageComplex>("/api/chatmessage", message);
  }

  public async Task<Response<ChatMessageComplex>> EditMessage(int messageId, string content)
  {
    return await HttpClient.Patch<ChatMessageComplex>($"/api/chatmessage/{messageId}/{content}");
  }

  public async Task<Response<ChatMessageComplex>> RemoveMessage(int id)
  {
    return await HttpClient.Delete<ChatMessageComplex>($"/api/chatmessage/{id}");
  }
}
