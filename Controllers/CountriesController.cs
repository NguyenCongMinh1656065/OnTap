using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ôn_tập.Data;
using Ôn_tập.Dto;
using Ôn_tập.Interfaces;
using Ôn_tập.Models;

namespace Ôn_tập.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountriesController(DataContext context , ICountryRepository countryRepository , IMapper mapper)
        {
            _context = context;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetCountries()
        {
            var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(countries);
        }
        [HttpGet("{id}")]
        public IActionResult GetCountry(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId))
                return NotFound();
            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountry(countryId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(country);
        }
        [HttpGet("/owner/{id}")]
        public IActionResult GetCountryOfAnOwner(int ownerId)
        {
            var country = _mapper.Map<CountryDto>(
                 _countryRepository.GetCountryByOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(country);
        }
        [HttpPut]
        public IActionResult UpdateCountry(int countryId, [FromBody] CountryDto updateCountry)
        {
          if (updateCountry == null)
                return BadRequest(ModelState);
          if (countryId != updateCountry.Id)
                return BadRequest(ModelState);
            if (!_countryRepository.CountryExists(countryId))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest();
            var countryMap = _mapper.Map<Country>(updateCountry);
            if (!_countryRepository.UpdateCountry(countryMap))
            {
                ModelState.AddModelError("", "somethiong wrong");
                return StatusCode(500, ModelState);
            }    
            return NoContent();
        }
        [HttpPost]
        public IActionResult CreateCountry([FromBody] CountryDto countryCreate)
        {
          if (countryCreate == null)
                return BadRequest(ModelState);
          var country = _countryRepository.GetCountries()
                .Where(c => c.Name.Trim().ToUpper() == countryCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (country != null)
            {
                ModelState.AddModelError("", "Country already exists");
                return StatusCode(422, ModelState);
            } 
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var countryMap = _mapper.Map<Country>(countryCreate);

            if (!_countryRepository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");

        }
        [HttpDelete("{id}")]
        public IActionResult DeleteCountry(int countryId) 
        {
           if  (!_countryRepository.CountryExists(countryId))
            {
                return NotFound();
            }
            var countryDelete = _countryRepository.GetCountry(countryId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if(!_countryRepository.DeleteCountry(countryDelete))
            {
                ModelState.AddModelError("", "Something went wrong while delete");
                return StatusCode(500, ModelState);
            }
            return Ok("successfully delete");
        }
    }
}
