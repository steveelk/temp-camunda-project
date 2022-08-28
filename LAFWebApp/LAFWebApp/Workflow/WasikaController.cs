using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LAFWebApp.Workflow
{
    [Route("Workflow/[controller]")]
    [ApiController]
    public class WasikaController : ControllerBase
    {
        // GET: api/<WasikaController>
        [Authorize()]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var test = HttpContext.User;

            return new string[] { "value1", "value2" };
        }

        // GET api/<WasikaController>/5
        [Authorize]
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "valuesteve";
        }

        // POST api/<WasikaController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<WasikaController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<WasikaController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
