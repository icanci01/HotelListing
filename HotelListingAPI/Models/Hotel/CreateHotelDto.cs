namespace HotelListingAPI.Models.Hotel;

public class CreateHotelDto
{
    public string Name { get; set; }
    public string Address { get; set; }
    public int CountryId { get; set; }
}