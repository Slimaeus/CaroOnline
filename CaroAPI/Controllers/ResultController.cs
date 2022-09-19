using AutoMapper;
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
        public IActionResult Post([FromBody] ResultRequest resultRequest)
        {
            var result = mapper.Map<ResultRequest, Result>(resultRequest);
            resultService.AddResult(result);

            return BadRequest();
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
