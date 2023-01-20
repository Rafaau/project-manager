using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Core.Interfaces;
public interface IUserCallService
{
  Task<Response<User[]>> GetAllUsers();
  Task<Response<User>> GetUserByEmail(string email);
  Task<Response<User>> AddUser(User user);
  Task<Response<UserSimplified>> UpdateUser(UserSimplified user);
}
