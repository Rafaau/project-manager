using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Core.Interfaces;
public interface IChatMessageCallService
{
  Task<Response<ChatMessageComplex[]>> GetAll();
  Task<Response<ChatMessageComplex[]>> GetByProjectId(int projectId);
  Task<Response<ChatMessageComplex>> AddMessage(ChatMessageRequest message);
  Task<Response<ChatMessageComplex>> EditMessage(int messageId, string content);
  Task<Response<ChatMessageComplex>> RemoveMessage(int id);
}
