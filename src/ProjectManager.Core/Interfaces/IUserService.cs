using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManager.Core.ProjectAggregate;

namespace ProjectManager.Core.Interfaces;
public interface IUserService
{
  Task<IQueryable<User>> RetrieveAllUsers();
  Task<User> RetrieveUserByEmail(string email);
  Task<User> CreateUser(User request);
  Task<User> UpdateUser(User request);
  Task<User> DeleteUser(int id);
}

