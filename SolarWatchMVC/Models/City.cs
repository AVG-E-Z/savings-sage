namespace SolarWatch.Model;

public class City
{
    public City(string name, double latitude, double longitude, string country, string state)
    {
        Name = name;
        Latitude = latitude;
        Longitude = longitude;
        Country = country;
        State = state;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Country { get; set; }
    public string? State { get; set; }
}