
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class RegionsController : ControllerBase
    {
     
        private readonly IRegionRepository _regionRepository;
        private readonly NZWalksDbContext _dbContext;
        private readonly IMapper _mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _regionRepository = regionRepository;
         

        }
        
        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            //Get Data From Database - Domain models
            var regionsDomain = await _regionRepository.GetAllAsync();
            //return Dtos
            return Ok(_mapper.Map<List<RegionDto>>(regionsDomain));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            // var region = dbContext.Regions.Find(id);
            //Get Region Domain Model From Database

            var regionDomain = await _regionRepository.GetByIdAsync(id);

            if(regionDomain == null)
            {
                return NotFound();
            }


            //Return DTO back to client
            return Ok(_mapper.Map<RegionDto>(regionDomain));
        }

        //POST to Create New Region
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // if(ModelState.IsValid){
            //Map or Convert DTO to Domain Model
            var regionDomainModel = _mapper.Map<Region>(addRegionRequestDto);

            //Use Domain Model to create Region
            regionDomainModel = await _regionRepository.CreateAsync(regionDomainModel);
            // await _dbContext.Regions.AddAsync(regionDomainModel);
            // await _dbContext.SaveChangesAsync();

            //Map Domain model back to DTO
            var regionDto = _mapper.Map<RegionDto>(regionDomainModel);

            return CreatedAtAction(nameof(GetById), new{id =regionDto.Id}, regionDto);
            // }
            // else
            // {
            //     return BadRequest(ModelState); 
            // }
           
        }

        //Update region
        //PUT 
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto )
        {
            // if(ModelState.IsValid)
            // {
            //Map DTO to Domain Model
            var regionDomainModel = _mapper.Map<Region>(updateRegionRequestDto);

            //Check if region exists dbcontext
            // var regionDomainModel = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            regionDomainModel = await _regionRepository.UpdateAsync(id, regionDomainModel);

            if(regionDomainModel == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<RegionDto>(regionDomainModel));
            // }

            // else {
            //     return BadRequest(ModelState);
            // }
         

             // //Map DTO to Domain model
            // regionDomainModel.Code = updateRegionRequestDto.Code;
            // regionDomainModel.Name = updateRegionRequestDto.Name;
            // regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            // await _dbContext.SaveChangesAsync();
        }

        //Delete Region
        //DELETE
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        { 
            var regionDomainModel = await _regionRepository.DeleteAsync(id);

            if(regionDomainModel == null)
            {
                return NotFound();
            }

            //return deleted Region back
           

            return Ok(_mapper.Map<RegionDto>(regionDomainModel));
        }
    }
}