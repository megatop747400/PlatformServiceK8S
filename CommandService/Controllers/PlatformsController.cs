using Microsoft.AspNetCore.Mvc;


namespace CommandService.Controllers
{
    [ApiController]
    [Route("api/c/[Controller]")]
    public class PlatformsController : ControllerBase
    {

        public class PlatformReadDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Publisher { get; set; }
            public string Cost { get; set; }
        }

        [HttpPost]
        public ActionResult TestPost(PlatformReadDto platform)
        {
           
            System.Console.WriteLine($"Post request was received in platforms controller {platform?.Name}");

            return Ok("all ok");
        }

    }
}