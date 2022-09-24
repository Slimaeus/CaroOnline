using Microsoft.AspNetCore.Mvc;
using Model.RequestModels;
using Service.APIServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CaroAPI.Controllers
{
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
        //[Authorize]
        [HttpGet("GetResults")]
        public IActionResult Get([FromQuery] PagingRequest pagingRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var results = _resultService.GetResults(pagingRequest);
            if (results.Succeeded)
                return Ok(results);
            return BadRequest(results);
        }
        [HttpGet("GetResultsByUserName")]
        public async Task<IActionResult> Get(string userName, [FromQuery] PagingRequest pagingRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var results = await _resultService.GetResultsByUserName(userName, pagingRequest);
            if (results.Succeeded)
                return Ok(results);
            return BadRequest(results);
        }

        // GET api/<ResultController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ResultController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ResultRequest resultRequest)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var serviceResult = await _resultService.AddResult(resultRequest);
            if (serviceResult.Succeeded)
                return Ok(serviceResult);
            return BadRequest(serviceResult);
        }

        // PUT api/<ResultController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

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
