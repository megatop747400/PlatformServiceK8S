using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.SyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;

namespace PlatformService.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _platformRepo;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;

        public PlatformsController(IPlatformRepo platformRepo, IMapper mapper, ICommandDataClient commandDataClient)
        {
            this._platformRepo = platformRepo;
            this._mapper = mapper;
            this._commandDataClient = commandDataClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            var platforms = _platformRepo.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platform = _platformRepo.GetPlatformById(id);

            if (platform != null)
            {
                return Ok(_mapper.Map<PlatformReadDto>(platform));
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto createDto)
        {
            var platformDto = _mapper.Map<Platform>(createDto);
            _platformRepo.CreatePlatform(platformDto);
            _platformRepo.SaveChanges();

            string result = "NotSet";
            try
            {
                result = await _commandDataClient.SendPlatformToCommand(_mapper.Map<PlatformReadDto>(platformDto));
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Request failed - {ex.Message}");
            }

            System.Console.WriteLine($"And the result was {result}");

            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformDto.Id }, platformDto);
            //return Ok(_mapper.Map<PlatformReadDto>(platformDto));

        }
    }
}