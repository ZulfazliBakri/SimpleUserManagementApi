using Microsoft.AspNetCore.Mvc;
using SimpleUserManagementApi.Models;
using SimpleUserManagementApi.Services;

namespace SimpleUserManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    // GET: /api/users
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyCollection<User>> GetAll()
    {
        return Ok(_userService.GetAll());
    }

    // GET: /api/users/1
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<User> GetById(int id)
    {
        var user = _userService.GetById(id);
        return user is null ? NotFound(new { message = $"User with ID {id} was not found." }) : Ok(user);
    }

    // POST: /api/users
    [HttpPost]
    [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<User> Create([FromBody] CreateUserRequest request)
    {
        if (_userService.EmailExists(request.Email))
            return Conflict(new { message = "A user with this email address already exists." });

        var user = _userService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    // PUT: /api/users/1
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<User> Update(int id, [FromBody] UpdateUserRequest request)
    {
        if (_userService.GetById(id) is null)
            return NotFound(new { message = $"User with ID {id} was not found." });

        if (_userService.EmailExists(request.Email, id))
            return Conflict(new { message = "Another user already uses this email address." });

        return _userService.Update(id, request, out var updatedUser)
            ? Ok(updatedUser)
            : NotFound(new { message = $"User with ID {id} was not found." });
    }

    // DELETE: /api/users/1
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        return _userService.Delete(id)
            ? NoContent()
            : NotFound(new { message = $"User with ID {id} was not found." });
    }
}
