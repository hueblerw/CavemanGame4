using System;
using System.Collections.Generic;
using UnityEngine;

public class World {

    private static World myInstance;
    public static int X;
    public static int Z;
    public const int DAYS_PER_YEAR = 120;

    private Tile[,] worldArray;
    public double maxElevationDifference;

    private static System.Random randy;

    // Singleton Constructor
    public static World getWorld()
    {
        randy = new System.Random();
        if (myInstance == null)
        {
            throw new Exception("World is null must pass world dimensions to create a new World!");
        }
        return myInstance;
    }

    public static World generateNewWorld(int x, int z, bool fromFile)
    {
        randy = new System.Random();
        if (!fromFile)
        {
            myInstance = new World(x, z);
        }
        return myInstance;
    }

    // Constructor
    private World(int x, int z)
    {
        X = x;
        Z = z;
        maxElevationDifference = 0.0;
        // Build the primary layers - all can be generated asyncronously
        // *** Start syncronously *** - not worth changing to async.
        worldArray = CreatePrimaryWorldArray();
        // ocean percentage - hill percentage together
        UpdateOceanAndHillPercents();
        // Generate River numbers that require hillPercents and elevations
        CalculateAllFlows();
        // generate 20 years of habitats

        // stash 10 / 20 year history???

    }

    private void CalculateAllFlows()
    {
        // syncronously first, but theoretically all river initialization can be down asyncronously if necessary
        for (int x = 0; x < X; x++)
        {
            for (int z = 0; z < Z; z++)
            {
                CalculateFlow(x, z);
            }
        }
    }

    private Tile[,] CreatePrimaryWorldArray()
    {
        Tile[,] worldTiles = new Tile[X, Z];
        // Each layer can be done asynchronusly - seems unnecessary so far.
        // Elevation
        double startingElevation = randy.NextDouble() * 10.0 - 5.0;
        double[,] elevationLayer = generateWorldLayer(-100.0, 100.0, 2.0, startingElevation, true);
        // Temps
        double startingHighTemp = randy.NextDouble() * 40.0 + 40.0;
        double[,] highTempLayer = generateWorldLayer(20.0, 100.0, 2.0, startingHighTemp, false);
        double[,] lowTempLayer = generateWorldLayer(-20.0, 70.0, 2.0, randy.NextDouble() * 40.0, false);
        double startingMidpoint = randy.NextDouble() * 30.0 + 45.0;
        double[,] midpointLayer = generateWorldLayer(45.0, 75.0, 2.0, startingMidpoint, true);
        // Variance
        double[,] varianceLayer = generateWorldLayer(0.0, 12.0, 1.0, randy.NextDouble() * 12.0, false);
        // Humidity
        double[][,] humiditySegments = new double[Humidity.HUMIDITY_SEGMENTS][,];
        for (int i = 0; i < Humidity.HUMIDITY_SEGMENTS; i++)
        {
            humiditySegments[i] = generateWorldLayer(0.0, 12.0, 1.0, randy.NextDouble() * 12.0, false);
        }

        // This can be done asynchronously once the above is completed - seems to halve world creation time.
        Parallel.For(X, x =>
        {
            for (int z = 0; z < Z; z++)
            {
                Humidity currentHumidity = Humidity.convertToObject(x, z, humiditySegments);
                worldTiles[x, z] = new Tile(elevationLayer[x, z], lowTempLayer[x, z], highTempLayer[x, z], midpointLayer[x, z], varianceLayer[x, z], currentHumidity);
                maxElevationDifference = checkForBiggerMaxDiff(x, z, elevationLayer);
            }
        });

        return worldTiles;
    }

    public double[,] generateWorldLayer(double min, double max, double maxChange, double startingValue, bool squared)
    {
        double[,] layer = new double[X, Z];
        layer[0, 0] = startingValue;
        layer = BuildTopRow(layer, min, max, maxChange, squared);
        layer = BuildLeftMostColumn(layer, min, max, maxChange, squared);
        layer = FillOutRemainingWorld(layer, min, max, maxChange, squared);
        return layer;
    }

    public Tile[,] getWorldArray()
    {
        return worldArray;
    }

    private void UpdateOceanAndHillPercents()
    {
        for (int x = 0; x < X; x++)
        {
            for(int z = 0; z < Z; z++)
            {
                CalculateOceanPercents(x, z);
                CalculateHillPercentage(x, z);
            }
        }
    }

    private double[,] FillOutRemainingWorld(double[,] layer, double min, double max, double maxChange, bool squared)
    {
        for (int i = 1; i < layer.GetLength(0); i++)
        {
            for (int j = 1; j < layer.GetLength(1); j++)
            {
                double change = CalculateChange(randy.NextDouble(), maxChange, squared);
                double average = (layer[i - 1, j] + layer[i, j - 1]) / 2.0;
                layer[i, j] = Math.Max(Math.Min(average + change, max), min);
            }
        }
        return layer;
    }

    private double[,] BuildLeftMostColumn(double[,] layer, double min, double max, double maxChange, bool squared)
    {
        for (int i = 1; i < layer.GetLength(1); i++)
        {
            double change = CalculateChange(randy.NextDouble(), maxChange, squared);
            layer[0, i] = Math.Max(Math.Min(layer[0, i - 1] + change, max), min);
        }
        return layer;
    }

    private double[,] BuildTopRow(double[,] layer, double min, double max, double maxChange, bool squared)
    {
        for (int i = 1; i < layer.GetLength(0); i++)
        {
            double change = CalculateChange(randy.NextDouble(), maxChange, squared);
            layer[i, 0] = Math.Max(Math.Min(layer[i - 1, 0] + change, max), min);
        }
        return layer;
    }

    private double CalculateChange(double randomDouble, double maxChange, bool squared)
    {
        double change = randomDouble;
        if (squared)
        {
            change = Math.Pow(change, 2);
        }
        change = change * maxChange * Support.randomSign();
        return change;
    }

    // Use the Terrain Heights to find Ocean Percents
    private void CalculateOceanPercents(/*Terrain theMap,*/ int x, int z)
    {
        // Debug.Log("Sea Level: " + seaLevel);
        double sum = 0f;
        double negSum = 0f;

        // THIS USES THE TERRAIN MAP ITSELF TO CALCULATE THE OCEAN PERCENTAGE
        /*
        int sum = 0;
        int count = 0;

        float ax = x * XMultiplier;
        float az = z * ZMultiplier;
        while (ax < (x + .9f) * XMultiplier)
        {
            az = z * ZMultiplier;
            while (az < (z + .9f) * ZMultiplier)
            {
                count++;
                if (theMap.terrainData.GetInterpolatedHeight(ax, az) < seaLevel)
                {
                    sum++;
                }
                az += (ZMultiplier / 10f);
            }
            ax += (XMultiplier / 10f);
        }
        */

        // THIS VERSION ESTIMATES BASED ON:  SUM OF ALL NEGS / ABS SUM OF ALL
        Vector2[] cellsAround = Support.GetCoordinatesAround(x, z, X, Z);
        double myElevation = worldArray[x, z].getElevation();
        for (int i = 0; i < cellsAround.Length; i++)
        {
            // Average it to estimate the vertex values.
            double value = (worldArray[(int)cellsAround[i].x, (int)cellsAround[i].y].getElevation() + myElevation) / 2.0;
            sum += Math.Abs(value);
            if (value < 0.0)
            {
                negSum += Math.Abs(value);
            }
        }

        // worldArray[x, z].oceanPercent = (double)sum / count;
        worldArray[x, z].setOceanPercent(Math.Round(negSum / sum, 2));
    }


    // Calculate the Hill Percentages as normal
    private void CalculateHillPercentage(int x, int z)
    {
        if (worldArray[x, z].getOceanPercent() != 1.0)
        {
            worldArray[x, z].setHillPercent((NetDiff(x, z) / maxElevationDifference));
        }
    }

    // Finds the max elevation difference between cells on the map.
    private double NetDiff(int x, int z, double[,] elevationLayer)
    {
        double diff = 0f;
        Vector2[] coor = Support.GetCoordinatesAround(x, z, X, Z);
        for (int i = 0; i < coor.Length; i++)
        {
            diff += Math.Abs((elevationLayer[x, z] - elevationLayer[(int)coor[i].x, (int)coor[i].y]));
        }

        return diff / coor.Length;
    }

    // Finds the max elevation difference between cells on the map.
    private double NetDiff(int x, int z)
    {
        double diff = 0f;
        Vector2[] coor = Support.GetCoordinatesAround(x, z, X, Z);
        for (int i = 0; i < coor.Length; i++)
        {
            diff += Math.Abs((worldArray[x, z].getElevation() - worldArray[(int)coor[i].x, (int)coor[i].y].getElevation()));
        }

        return diff / coor.Length;
    }

    private double checkForBiggerMaxDiff(int x, int z, double[,] elevationLayer)
    {
        double diff = NetDiff(x, z, elevationLayer);
        if (diff > maxElevationDifference)
        {
            return diff;
        }
        else
        {
            return maxElevationDifference;
        }
    }

    // Calculate the Flow direction and flow rate
    private void CalculateFlow(int x, int z)
    {
        if (worldArray[x, z].getOceanPercent() == 1.00)
        {
            worldArray[x, z].initializeLocalWater("none", 1.0, 0.0);
        }
        else
        {
            // Get downstream options
            List<Vector3> coor = Support.GetCardinalCoordinatesAround(x, z, X, Z);
            int len = coor.Count;
            for (int v = 0; v < coor.Count; v++)
            {
                // Debug.Log(worldArray[(int)coor[v].x, (int)coor[v].y].elevation + " >= " + worldArray[x, z].elevation);
                if (worldArray[(int)coor[v].x, (int)coor[v].y].getElevation() >= worldArray[x, z].getElevation())
                {
                    coor.Remove(coor[v]);
                    v--;
                }
            }
            Vector3[] downhills = coor.ToArray();
            // Choose a random downstream IF there is a choice
            string downstreamDirection = "none";
            double flowRateMultiplier = 1.0;
            if (downhills.Length != 0)
            {
                int index = 0;
                if (downhills.Length > 1)
                {
                    index = randy.Next(0, downhills.Length);
                }
                downstreamDirection = SetDirection(downhills[index]);
                worldArray[x, z].initializeLocalWater(downstreamDirection, CalculateSoilAbsorption(x, z), flowRateMultiplier);
                // add reverse direction to tile in same direction of you flow
                worldArray[(int) coor[index].x, (int) coor[index].z].getLocalWater().addUpstreamDirection(setReverseDirection(downstreamDirection));
            }
        }
    }

    private string SetDirection(Vector3 direction)
    {
        string output = "none";
        switch ((int)direction.z)
        {
            case 0:
                output = "left";
                break;
            case 1:
                output = "right";
                break;
            case 2:
                output = "down";
                break;
            case 3:
                output = "up";
                break;
        }

        return output;
    }

    private string setReverseDirection(string direction)
    {
        switch (direction)
        {
            case "left":
                return "right";
            case "right":
                return "left";
            case "down":
                return "up";
            case "up":
                return "down";
        }
        throw new Exception("Asking for a reverse direction, but [" + direction + "] not a valid direction!");
    }

    private double CalculateSoilAbsorption(int x, int z)
    {
        return ((randy.NextDouble() + .2) * (1.0 - worldArray[x, z].getOceanPercent()) * (1.0 - worldArray[x, z].getHillPercent()));
    }

}
