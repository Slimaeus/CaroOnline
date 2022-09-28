using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.RequestModels;
using Service.APIServices;

namespace CaroAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        /// <summary>
        /// Get all Roles
        /// </summary>
        /// <returns>Role List</returns>
        [AllowAnonymous]
        [HttpGet("get-list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetList()
        {
            var result = await _roleService.GetList();
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }
        /// <summary>
        /// Create a Role
        /// </summary>
        /// <param name="roleRequest">Role Create Model</param>
        /// <returns>Create Status</returns>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] RoleRequest roleRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _roleService.Create(roleRequest);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
