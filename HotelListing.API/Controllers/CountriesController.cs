﻿using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Models.Country;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountriesController : ControllerBase
{
    private readonly ICountriesRepository _countriesRepository;
    private readonly IMapper _mapper;

    public CountriesController(IMapper mapper, ICountriesRepository countriesRepository)
    {
        _mapper = mapper;
        _countriesRepository = countriesRepository;
    }

    // GET: api/Countries
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
    {
        var countries = await _countriesRepository.GetAllAsync();
        var records = _mapper.Map<List<GetCountryDto>>(countries);
        return Ok(records);
    }

    // GET: api/Countries/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CountryDto>> GetCountry(int id)
    {
        var country = await _countriesRepository.GetDetails(id);

        if (country == null) return NotFound();

        var record = _mapper.Map<CountryDto>(country);

        return record;
    }

    // PUT: api/Countries/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountry)
    {
        if (id != updateCountry.Id) return BadRequest("Invalid Record Id");

        var country = await _countriesRepository.GetAsync(id);

        if (country == null) return NotFound();

        _mapper.Map(updateCountry, country);

        try
        {
            await _countriesRepository.UpdateAsync(country);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await CountryExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Countries
    [HttpPost]
    public async Task<ActionResult<Country>> PostCountry(CreateCountryDto createCountry)
    {
        var country = _mapper.Map<Country>(createCountry);

        await _countriesRepository.AddAsync(country);

        return CreatedAtAction("GetCountry", new { id = country.Id }, country);
    }

    // DELETE: api/Countries/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        var country = await _countriesRepository.GetAsync(id);

        if (country == null) return NotFound();

        await _countriesRepository.DeleteAsync(id);

        return NoContent();
    }

    private async Task<bool> CountryExists(int id)
    {
        return await _countriesRepository.Exists(id);
    }
}