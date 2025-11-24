using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Blazor.Application.Features.Delivery.Coordinates.Models;

public class Coordinate
{
    public double Latitude { get; set; } = 32.8877;
    public double Longitude { get; set; } = 13.1872;

    public override string ToString()
    {
        return $"{Latitude},{Longitude}";
    }

    public static Coordinate FromString(string coordinateString)
    {
        if (string.IsNullOrEmpty(coordinateString))
            return null;

        var parts = coordinateString.Split(',');
        if (parts.Length == 2 &&
            double.TryParse(parts[0], out var lat) &&
            double.TryParse(parts[1], out var lng))
        {
            return new Coordinate { Latitude = lat, Longitude = lng };
        }
        return null;
    }
}
