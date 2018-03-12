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

    public void setStats(string downstream, double soilAbsorption, double flowRateMultiplier)
    {
        downstreamDirection = downstream;
        this.soilAbsorption = soilAbsorption;
        this.flowRateMultiplier = flowRateMultiplier;
    }

    public void addUpstreamDirection(string direction)
    {
        upstreamDirections.Add(direction);
    }

    public double getFlowRateMultiplier()
    {
        return flowRateMultiplier;
    }

    public double getSoilAbsorption()
    {
        return soilAbsorption;
    }

    public string getDownstreamDirection()
    {
        return downstreamDirection;
    }

    public List<string> getUpstreamDirections()
    {
        return upstreamDirections;
    }

}