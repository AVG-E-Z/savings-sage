namespace SolarWatch.Model;

public class Coordinate(double latitude, double longitude)
{
    public double Latitude { get; init; } = latitude;
    public double Longitude { get; init; } = longitude;
}