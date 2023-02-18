using System.Diagnostics;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.Logging;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.Core.ProjectAggregate.Specifications;
using ProjectManager.SharedKernel.Interfaces;

namespace ProjectManager.Core.Services;
public class UserService : IUserService
{
  private readonly IRepository<User> _userRepository;
  private readonly IRepository<Project2> _projectRepository;
  private readonly ILoggerAdapter<UserService> _logger;
  public UserService(
    IRepository<User> userRepository, 
    IRepository<Project2> projectRepository,
    ILoggerAdapter<UserService> logger)
  {
    _userRepository = userRepository;
    _projectRepository = projectRepository;
    _logger = logger;
  }
  public async Task<IQueryable<User>> RetrieveAllUsers()
  {
    _logger.LogInformation("Retrieving all users");
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var users = await _userRepository.ListAsync();

      return users.AsQueryable();
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while retrieving all users");
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("Users retrieved in {0}ms", stopWatch.ElapsedMilliseconds);
    }
  }

  public async Task<User> RetrieveUserByEmail(string email)
  {
    _logger.LogInformation("Retrieving user with email: {0}", email);
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var userSpec = new UserByEmail(email);
      var user = await _userRepository.FirstOrDefaultAsync(userSpec);

      return user;
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while retrieving user with email: {0}", email);
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("User with email: {0} retrieved in {1}ms", email, stopWatch.ElapsedMilliseconds);
    }
  }

  public async Task<User> CreateUser(User request)
  {
    _logger.LogInformation("Creating user");
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var createdUser = await _userRepository.AddAsync(request);

      return createdUser;
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while creating user");
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("User created in {0}ms", stopWatch.ElapsedMilliseconds);
    }
  }

  public async Task<User> UpdateUser(User request)
  {
    _logger.LogInformation("Updating user (id: {0})", request.Id);
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var userSpec = new UserById(request.Id);
      var userToUpdate = await _userRepository.FirstOrDefaultAsync(userSpec);

      userToUpdate.Firstname = request.Firstname;
      userToUpdate.Lastname = request.Lastname;
      userToUpdate.Email = request.Email;
      userToUpdate.Password = request.Password;
      userToUpdate.Role = request.Role;

      await _userRepository.UpdateAsync(userToUpdate);
      return userToUpdate;
    }
    catch (Exception e)
    {
      _logger.LogError(e, $"Something went wrong while updating user (id: {request.Id})");
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("User (id: {0}) updated in {1}ms", request.Id, stopWatch.ElapsedMilliseconds);
    }
  }

  public async Task<User> DeleteUser(int id)
  {
    _logger.LogInformation("Deleting user (id: {0})", id);
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var userSpec = new UserById(id);
      var userToDelete = await _userRepository.FirstOrDefaultAsync(userSpec);

      await _userRepository.DeleteAsync(userToDelete);
      return userToDelete;
    }
    catch (Exception e)
    {
      _logger.LogError(e, $"Something went wrong while deleting user (id: {id})");
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("Deleted user (id: {0}) in {1}ms", id, stopWatch.ElapsedMilliseconds);
    }
  }
}
