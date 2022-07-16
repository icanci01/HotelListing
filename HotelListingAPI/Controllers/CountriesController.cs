using AutoMapper;
using HotelListingAPI.Data;
using HotelListingAPI.Models.Country;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelListingAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountriesController : ControllerBase
{
    private readonly HotelListingDbContext _context;
    private readonly IMapper _mapper;

    public CountriesController(HotelListingDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: api/Countries
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
    {
        var countries = await _context.Countries.ToListAsync();
        var records = _mapper.Map<List<GetCountryDto>>(countries);
        return Ok(records);
    }

    // GET: api/Countries/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CountryDto>> GetCountry(int id)
    {
        var country = await _context.Countries.Include(q => q.Hotels)
            .FirstOrDefaultAsync(q => q.Id == id);

        if (country == null) return NotFound();

        var record = _mapper.Map<CountryDto>(country);

        return record;
    }

    // PUT: api/Countries/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountry)
    {
        if (id != updateCountry.Id) return BadRequest("Invalid Record Id");

        var country = await _context.Countries.FindAsync(id);

        if (country == null) return NotFound();

        _mapper.Map(updateCountry, country);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CountryExists(id))
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
        _context.Countries.Add(country);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCountry", new { id = country.Id }, country);
    }

    // DELETE: api/Countries/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        if (_context.Countries == null) return NotFound();
        var country = await _context.Countries.FindAsync(id);
        if (country == null) return NotFound();

        _context.Countries.Remove(country);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CountryExists(int id)
    {
        return (_context.Countries?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}