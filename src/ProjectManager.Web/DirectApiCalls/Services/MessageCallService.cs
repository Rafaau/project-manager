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
public class MessageCallService : ServiceBase, IMessageCallService
{
  public MessageCallService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
  {
  }
  public async Task<Response<MessageComplex[]>> GetAll()
  {
    return await HttpClient.GetResponse<MessageComplex[]>("/api/message");
  }

  public async Task<Response<MessageComplex[]>> GetByProjectId(int projectId)
  {
    return await HttpClient.GetResponse<MessageComplex[]>($"/api/message?$filter=projectId eq {projectId}");
  }

  public async Task<Response<MessageComplex>> AddMessage(MessageRequest message)
  {
    return await HttpClient.Post<MessageRequest, MessageComplex>("/api/message", message);
  }

  public async Task<Response<MessageComplex>> RemoveMessage(int id)
  {
    return await HttpClient.Delete<MessageComplex>($"/api/message/{id}");
  }
}
