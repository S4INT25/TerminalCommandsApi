using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TerminalCommandsApi.Controllers
{
    
    public record TestRecord(string Name, int Age = 45)
    {
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    };
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase

    {

        [HttpGet("")]
        public ActionResult TestRecords()
        {
            var result = new TestRecord("Nice", 56);
            return Ok(result.ToString());
        }


    }
}