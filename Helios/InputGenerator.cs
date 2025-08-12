using Helios.Data;

namespace Helios;

public class InputGenerator
{
    private static string[] _cities = [
        "Shanghai",
        "Hong Kong",
        "New York",
        "Seoul",
        "Mexico City",
        "London",
    ];

    public static InputData Generate(int count = 10)
    {
        var inputData = new InputData();

        var rnd = new Random();
        for (int i = 0; i < count; i++)
        {
            inputData.WeatherMeasures.Add(new WeatherMeasure
            {
                ID = i,
                City = _cities[rnd.Next(_cities.Length)],
                Temperature = (float)rnd.NextDouble() * 30.0f,
            });
        }

        return inputData;
    }
}