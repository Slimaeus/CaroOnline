using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.DbModels;
using Model.RequestModels;
using Service.APIServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CaroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultController : ControllerBase
    {
        private readonly IResultService resultService;
        private readonly IMapper mapper;

        // GET: api/<ResultController>
        public ResultController(IResultService resultService, IMapper mapper)
        {
            this.resultService = resultService;
            this.mapper = mapper;
        }
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            var results = resultService.GetResults();
            return Ok(results);
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
            var serviceResult = await resultService.AddResult(resultRequest);
            if (serviceResult.Succeeded)
                return Ok(serviceResult);
            return BadRequest(serviceResult);
        }

        // PUT api/<ResultController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ResultController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
