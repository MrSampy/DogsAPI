using API.Middleware.Model;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions.DTOs;
using Services.Abstractions.Interfaces;
using System.Text.Json;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("")]
    public class DogController(IDogService dogService) : ControllerBase
    {
        private readonly IDogService _dogService = dogService;

        [HttpGet("ping")]
        [RateLimit(MaxRequests = 10, TimeWindowInSeconds = 1)]
        public IActionResult Ping()
        {
            return Ok("Dogshouseservice.Version1.0.1");
        }
        
        [HttpGet("dogs")]
        [RateLimit(MaxRequests = 10, TimeWindowInSeconds = 1)]
        public async Task<IActionResult> GetDogs([FromQuery] string attribute = "name", [FromQuery] string order = "asc", [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default) 
        {
            var dogs = await _dogService.GetAllAsync(attribute, order, pageNumber, pageSize, cancellationToken);

            return Ok(dogs);
        }

        [HttpPost("dog")]
        [RateLimit(MaxRequests = 10, TimeWindowInSeconds = 1)]
        public async Task<IActionResult> CreateDog([FromBody]DogDTO dog, CancellationToken cancellationToken = default) 
        {
            await _dogService.Insert(dog, cancellationToken);
            return Ok();
        }
    }
}
