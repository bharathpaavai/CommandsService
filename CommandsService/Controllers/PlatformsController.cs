using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;

        public PlatformsController(ICommandRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public ActionResult<IEnumerable<CommandReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting All platforms");
            var platform = _repository.GetAllPlatforms();
            return Ok (_mapper.Map<PlatformReadDto>(platform));
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound POST # Command Service");
            return Ok("inbound test of from platforms controller");
        }
    }
}
