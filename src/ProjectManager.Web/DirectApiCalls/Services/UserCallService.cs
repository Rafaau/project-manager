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
public class UserCallService : ServiceBase, IUserCallService
{
  public UserCallService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
  {
  }

  public async Task<Response<User[]>> GetAllUsers()
  {
    return await HttpClient.GetResponse<User[]>("api/user");
  }

  public async Task<Response<User>> GetUserByEmail(string email)
  {
    return await HttpClient.GetResponse<User>($"api/user/{email}");
  }

  public async Task<Response<User>> AddUser(User user)
  {
    return await HttpClient.Post<User, User>("api/user", user);
  }

  public async Task<Response<UserSimplified>> UpdateUser(UserSimplified user)
  {
    return await HttpClient.Put<UserSimplified, UserSimplified>("api/user", user);
  } 
}
