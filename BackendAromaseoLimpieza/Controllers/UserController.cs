using BackendAromaseoLimpieza.Interfaces;
using BackendAromaseoLimpieza.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace BackendAromaseoLimpieza.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateUser(User user)
    {
        var result = await _userService.CreateUser(user);
        return Ok(result);
    }
    
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var result = await _userService.GetUserById(id);
        return Ok(result);
    }
    
    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> UpdateUser(User user)
    {
        var result = await _userService.UpdateUser(user);
        return Ok(result);
    }
    
    [HttpPost]
    [Route("page/{page}/{pageSize}")]
    public async Task<IActionResult> GetUsers([FromRoute] int page, [FromRoute] int pageSize, UserFilter filter)
    {
        var result = await _userService.GetUsers(page, pageSize, filter);       
        return Ok(result);
    }

}