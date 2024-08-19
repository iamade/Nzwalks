
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            _walkRepository = walkRepository;
            _mapper = mapper;

        }
        // CREATE WALK
        // POST: /api/walk
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto) 
        {

            //Map DTO to Domain Model
            var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDto);
            await _walkRepository.CreateAsync(walkDomainModel);

            //Map Domain model to DTO
            return Ok(_mapper.Map<WalkDto>(walkDomainModel));

        }

        //GET Walks
        // GET: /api/walks?filterOn=Name&filterQuery=Track&aortBy=Name&isAscending=true&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string filterOn, [FromQuery] string filterQuery, [FromQuery] string sortBy, 
                                                [FromQuery] bool isAscending, [FromQuery] int  pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var walksDomainModel = await _walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);

            //Map Domain Models to DTO
            return Ok(_mapper.Map<List<WalkDto>>(walksDomainModel));
        }

        //Get Walk By Id
        //GET: /api/walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await _walkRepository.GetByIdAsync(id);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            //Map Domain Model to DTO
            return Ok(_mapper.Map<WalkDto>(walkDomainModel));
        }

        // Update walk by id
        // PUT: /api/Walks/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto) 
        {
            // if(ModelState.IsValid)
            // {
            // Map DTO to Domain Model
            var walkDomainModel= _mapper.Map<Walk>(updateWalkRequestDto);
            walkDomainModel = await _walkRepository.UpdateAsync(id, walkDomainModel);
            if(walkDomainModel == null) {
                return NotFound();
            }
            
            //Map Domain Model to Dto
            return Ok(_mapper.Map<WalkDto>(walkDomainModel));
            // }
            // else{
            //     return BadRequest(ModelState);
            // }
           

        }

        // Delete a Walk By Id
        // DELETE: /api/Walks/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id )
        {
            var deletedWalkDomainModel = await _walkRepository.DeletedAsync(id);
            if(deletedWalkDomainModel == null){
                return NotFound();
            }

            //Map Domain Model to Dto
            return Ok(_mapper.Map<WalkDto>(deletedWalkDomainModel));
        } 
 
}
}