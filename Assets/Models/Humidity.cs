

using System;

public class Humidity {

    public const int HUMIDITY_SEGMENTS = 10;
    public const int DAYS_PER_SEGMENT = World.DAYS_PER_YEAR / HUMIDITY_SEGMENTS;
    private double[] segments;

    public Humidity(double[] segments)
    {
        this.segments = segments;
    }

    public void setSegments(double[] segments)
    {
        this.segments = segments;
    }

    public double[] getSegments()
    {
        return segments;
    }

    public double getHumidityForDay(int day)
    {
        return 0.0;
    }

    public static Humidity convertToObject(int x, int z, double[][,] humiditySegments)
    {
        double[] incomingSegments = new double[HUMIDITY_SEGMENTS];
        for (int i = 0; i < humiditySegments.Length; i++)
        {
            incomingSegments[i] = Math.Round(humiditySegments[i][x, z], 1);
        }

        return new Humidity(incomingSegments);
    }
}
