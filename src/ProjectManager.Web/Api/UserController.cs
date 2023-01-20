using AutoMapper;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.Core.ProjectAggregate.Enums;
using ProjectManager.Core.ProjectAggregate.Specifications;
using ProjectManager.SharedKernel;
using ProjectManager.SharedKernel.Interfaces;
using ProjectManager.Web.ApiModels;
using Microsoft.AspNetCore.OData.Query;

namespace ProjectManager.Web.Api;

public class UserController : BaseApiController
{
  private readonly IUserService _userService;
  private readonly IMapper _mapper;

  public UserController(IMapper mapper, IUserService userService)
  {
    _mapper = mapper;
    _userService = userService;
  }

  [HttpGet]
  public async Task<IActionResult> List(ODataQueryOptions<User> queryOptions)
  {
    try
    {
      var retrievedUsers =
        queryOptions.ApplyTo(await _userService.RetrieveAllUsers());

      return Ok(_mapper.Map<UserComplex[]>(retrievedUsers).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }

  [HttpGet("{email}")]
  public async Task<IActionResult> GetByEmail(string email)
  {
    try
    {
      var retrievedUser =
        await _userService.RetrieveUserByEmail(email);

      if (retrievedUser is null)
        return NotFound();

      return Ok(_mapper.Map<UserComplex>(retrievedUser).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }

  [HttpPost]
  public async Task<IActionResult> Post([FromBody] UserRequest request)
  {
    try
    {
      var mapped = _mapper.Map<User>(request);

      var createdUser = await _userService.CreateUser(mapped);

      return CreatedAtAction(nameof(GetByEmail), new { createdUser.Email }, _mapper.Map<UserSimplified>(createdUser).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }

  [HttpPut]
  public async Task<IActionResult> Update([FromBody] UserSimplified request)
  {
    try
    {
      var mapped = _mapper.Map<User>(request);

      var updatedUser = await _userService.UpdateUser(mapped);

      return Ok(_mapper.Map<UserSimplified>(updatedUser).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
  {
    try
    {
      var deletedUser = await _userService.DeleteUser(id);

      if (deletedUser is null)
        return NotFound();

      return Ok(_mapper.Map<UserComplex>(deletedUser).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }

}
