using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManager.Core.ProjectAggregate;

namespace ProjectManager.Core.Interfaces;
public interface IMessageService
{
  Task<IQueryable<Message>> RetrieveAllMessages();
  Task<Message> PostMessage(Message request);
  Task<Message> EditMessage(Message request);
  Task<Message> DeleteMessage(int id);
}
