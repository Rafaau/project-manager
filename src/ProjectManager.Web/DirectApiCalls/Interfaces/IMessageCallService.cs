using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Core.Interfaces;
public interface IMessageCallService
{
  Task<Response<MessageComplex[]>> GetAll();
  Task<Response<MessageComplex[]>> GetByProjectId(int projectId);
  Task<Response<MessageComplex>> AddMessage(MessageRequest message);
  Task<Response<MessageComplex>> RemoveMessage(int id);
}
