using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using ProjectManager.Core.ProjectAggregate;

namespace ProjectManager.Web.Authentication;

public class AuthStateProvider : AuthenticationStateProvider
{
  private readonly ProtectedSessionStorage _sessionStorage;
  private readonly ProtectedLocalStorage _localStorage;
  private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

  public AuthStateProvider(ProtectedSessionStorage sessionStorage, ProtectedLocalStorage localStorage)
  {
    _sessionStorage = sessionStorage;
    _localStorage = localStorage;
  }

  public override async Task<AuthenticationState> GetAuthenticationStateAsync()
  {
    try
    {
      var userSessionStorageResult = await _sessionStorage.GetAsync<User>("UserSession");
      var userLocalStorageResult = await _localStorage.GetAsync<User>("UserLocal");
      var userSession = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;
      var userLocal = userLocalStorageResult.Success ? userLocalStorageResult.Value : null;

      if (userSession == null && userLocal == null)
        return await Task.FromResult(new AuthenticationState(_anonymous));

      var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
      {
        new Claim(ClaimTypes.Name, userSession != null ? (userSession.Firstname + " " + userSession.Lastname) : (userLocal.Firstname + " " + userLocal.Lastname)),
        new Claim(ClaimTypes.Role, userSession != null ? (userSession.Role.ToString()) : (userLocal.Role.ToString())),
        new Claim(ClaimTypes.Email, userSession != null ? (userSession.Email) : (userLocal.Email))
      }, "CustomAuth"));
      return await Task.FromResult(new AuthenticationState(claimsPrincipal));
    }
    catch
    {
      return await Task.FromResult(new AuthenticationState(_anonymous));
    }
  }

  public async Task UpdateAuthenticationState(User user, bool remember)
  {
    ClaimsPrincipal claimsPrincipal;

    if (user != null)
    {
      if (remember)
      {
        await _localStorage.SetAsync("UserLocal", user);
        claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
      {
        new Claim(ClaimTypes.Name, user.Firstname + " " + user.Lastname),
        new Claim(ClaimTypes.Role, user.Role.ToString()),
        new Claim(ClaimTypes.Email, user.Email)
      }));
      }
      else
      {
        await _sessionStorage.SetAsync("UserSession", user);
        claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
      {
        new Claim(ClaimTypes.Name, user.Firstname + " " + user.Lastname),
        new Claim(ClaimTypes.Role, user.Role.ToString()),
        new Claim(ClaimTypes.Email, user.Email)
      }));
      }
    }
    else
    {
      await _localStorage.DeleteAsync("UserLocal");
      await _sessionStorage.DeleteAsync("UserSession");
      claimsPrincipal = _anonymous;
    }


    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
  }
}
