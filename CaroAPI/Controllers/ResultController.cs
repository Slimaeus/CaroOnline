using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.RequestModels;
using Service.APIServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CaroAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ResultController : ControllerBase
    {
        private readonly IResultService _resultService;

        // GET: api/<ResultController>
        public ResultController(IResultService resultService)
        {
            _resultService = resultService;
        }
        /// <summary>
        /// Get All Results in this game
        /// </summary>
        /// <param name="pagingRequest">Paging Resquest</param>
        /// <returns></returns>
        [HttpGet("GetList")]
        public IActionResult Get([FromQuery] PagingRequest pagingRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var results = _resultService.GetResults(pagingRequest);
            if (results.Succeeded)
                return Ok(results);
            return BadRequest(results);
        }
        /// <summary>
        /// Get Results by UserName
        /// </summary>
        /// <param name="userName">UserName</param>
        /// <param name="pagingRequest">Paging Request</param>
        /// <returns>Result List</returns>
        [HttpGet("GetByUserName")]
        public async Task<IActionResult> Get(string userName, [FromQuery] PagingRequest pagingRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var results = await _resultService.GetResultsByUserName(userName, pagingRequest);
            if (results.Succeeded)
                return Ok(results);
            return BadRequest(results);
        }
        /// <summary>
        /// Create a result
        /// </summary>
        /// <param name="resultRequest">Create Result Request</param>
        /// <returns>Create Status</returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Post([FromBody] ResultRequest resultRequest)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var serviceResult = await _resultService.AddResult(resultRequest);
            if (serviceResult.Succeeded)
                return Ok(serviceResult);
            return BadRequest(serviceResult);
        }
        /// <summary>
        /// Delete Results by UserName
        /// </summary>
        /// <param name="userName">UserName</param>
        /// <param name="deleteResultRequest">Delete Result Request</param>
        /// <returns>Delete Status</returns>
        [HttpDelete("DeleteByUserName/{userName}")]
        public async Task<IActionResult> DeleteByUserName(string userName, DeleteResultRequest deleteResultRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var deleteResult = await _resultService.DeleteResultByUserName(userName, deleteResultRequest);
            if (deleteResult.Succeeded)
                return Ok(deleteResult);
            return BadRequest();
        }
        /// <summary>
        /// Delete Results by Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="deleteResultRequest">Delete Request Params</param>
        /// <returns>Delete Status</returns>
        [HttpDelete("DeleteById/{id}")]
        public async Task<IActionResult> DeleteById(Guid id, DeleteResultRequest deleteResultRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var deleteResult = await _resultService.DeleteResultById(id, deleteResultRequest);
            if (deleteResult.Succeeded)
                return Ok(deleteResult);
            return BadRequest();
        }
    }
}
