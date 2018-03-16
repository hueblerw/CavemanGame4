using System;

public class Tile {

    // Primary tile stats - from which the others are generated
    private double elevation;
    private int lowTemp;
    private int highTemp;
    private double midpoint;
    private double variance;
    private Humidity humidity;
    // secondary semi-permanent tiles stats
    private TempEquation tempEquation;
    private double oceanPercent;
    private double hillPercent;
    // Local variable stats
    // private Habitat habitat;
    private LocalWater localwater;
    private HashMap<string, List<object[]>> tileHistory;

    private System.Random randy = new System.Random();

	public Tile(double elevation, double lowTemp, double highTemp, double midpoint, double variance, Humidity humidity)
    {
        this.elevation = Math.Round(elevation, 2);
        this.lowTemp = (int) Math.Round(lowTemp, 0);
        this.highTemp = (int) Math.Round(highTemp, 0);
        this.midpoint = Math.Round(midpoint, 1);
        this.variance = Math.Round(variance, 2);
        this.humidity = humidity;
        // Use the above to generate the secondary the tempEq - THIS IS THE TASK THAT TAKES SOME TIME IN INITIAL GENERATION.
        tempEquation = new TempEquation(this.highTemp, this.lowTemp, midpoint, variance);
        randy = new System.Random();
        localwater = new LocalWater();
    }

    public void generateYearOfTemps()
    {
        int[] temps = new int[World.DAYS_PER_YEAR];
        for (int d = 0; d < World.DAYS_PER_YEAR; d++)
        {
            temps[d] = tempEquation.generateTodaysTemp(d, randy);
        }
        addToHistory("temps", temps);
    }

    public double getElevation()
    {
        return elevation;
    }

    public int getLowTemp()
    {
        return lowTemp;
    }

    public int getHighTemp()
    {
        return highTemp;
    }

    public double getMidpoint()
    {
        return midpoint;
    }

    public double getVariance()
    {
        return variance;
    }

    public Humidity getHumidity()
    {
        return humidity;
    }

    public double getOceanPercent()
    {
        return oceanPercent;
    }

    public double getHillPercent()
    {
        return hillPercent;
    }

    public LocalWater getLocalWater()
    {
        return localwater;
    }

    public void initializeLocalWater(string downstream, double soilAbsortion, double flowRateMultiplier)
    {
        localwater.setStats(downstream, soilAbsortion, flowRateMultiplier);
    }

    public void setOceanPercent(double oceanPercent)
    {
        this.oceanPercent = Math.Round(oceanPercent, 2);
    }

    public void setHillPercent(double hillPercent)
    {
        this.hillPercent = Math.Round(hillPercent, 2);
    }

    // this wont work but it is an example of the desired behavior
    private void addToHistory(string key, object[] array)
    {
        if (tileHistory.get(key) == null)
        {
            tileHistory.put(key, new List<array.Class>();
        }
        // 20 should he stored as a constant in habitat or tile memory
        if (tileHistory.get(key).Count < 20)
        {
            tileHistory.get(key).add(array);
        }
        else
        {
            tileHistory.get(key)[0].remove();
            tileHistory.get(key).add(array);
        }
    }

}
