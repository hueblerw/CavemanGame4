using System.Collections.Generic;

public class LocalWater
{
    private double flowRateMultiplier;  // needs elevation diff between me and downstream tile
    private double soilAbsorption;  // needs hilliness
    private List<string> upstreamDirections;  // needs surrounding elevations
    private string downstreamDirection;  // needs surrounding elevations

    private double volume;

    public LocalWater()
    {
        upstreamDirections = new List<string>();
    }

    public void setStats(string downstream, double soilAbsortion, double flowRateMultiplier)
    {
        downstreamDirection = downstream;
        this.soilAbsorption = soilAbsorption;
        this.flowRateMultiplier = flowRateMultiplier;
    }

    public void addUpstreamDirection(string direction)
    {
        upstreamDirections.Add(direction);
    }

}