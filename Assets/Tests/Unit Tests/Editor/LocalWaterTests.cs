using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LocalWaterTests {

	[Test]
	public void UpstreamDownstreamListsMatch() {
        int x = 40;
        int z = 40;
        World testWorld = World.generateNewWorld(x, z, false);
        Tile[,] worldArray = World.getWorld().getWorldArray();
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < z; j++)
            {
                List<string> upstream = worldArray[i, j].getLocalWater().getUpstreamDirections();
                if (upstream.Count > 0)
                {
                    foreach (string direction in upstream)
                    {
                        string reverseDirection = Support.setReverseDirection(direction);
                        Vector2 coor = Support.directionToCoor(direction, i, j);
                        Debug.Log(direction + " - " + i + ", " + j + " / " + coor.ToString());
                        Assert.AreEqual(reverseDirection, worldArray[(int) coor.x, (int) coor.y].getLocalWater().getDownstreamDirection());
                    }
                }
            }
        }
	}

    [Test]
    public void DownstreamIsHigherThanTarget()
    {
        int x = 40;
        int z = 40;
        double[,] elevationArray = new double[x, z];
        World testWorld = World.generateNewWorld(x, z, false);
        Tile[,] worldArray = World.getWorld().getWorldArray();
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < z; j++)
            {
                string direction = worldArray[i, j].getLocalWater().getDownstreamDirection();
                elevationArray[i, j] = worldArray[i, j].getElevation();
                // null is a ocean, none is a lake
                if (direction != null && direction != "none")
                {
                    Vector2 coor = Support.directionToCoor(direction, i, j);
                    Assert.GreaterOrEqual(worldArray[i, j].getElevation(), worldArray[(int)coor.x, (int)coor.y].getElevation());
                }
            }
        }
        ArrayPrinter.print(elevationArray, "Elevation Map: ");
    }

    [Test]
    public void IfLakeAllSurroundingAreHigher()
    {
        // Use the Assert class to test conditions.
        int x = 40;
        int z = 40;
        World testWorld = World.generateNewWorld(x, z, false);
        Tile[,] worldArray = World.getWorld().getWorldArray();
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < z; j++)
            {
                string direction = worldArray[i, j].getLocalWater().getDownstreamDirection();
                if (direction == "none")
                {
                    List<Vector3> coors = Support.GetCardinalCoordinatesAround(i, j, x, z);
                    foreach (Vector3 coor in coors)
                    {
                        Assert.GreaterOrEqual(worldArray[(int)coor.x, (int)coor.y].getElevation(), worldArray[i, j].getElevation());
                    }
                }
            }
        }
    }

    [Test]
    public void flowValuesAreWithinBounds()
    {
        int x = 40;
        int z = 40;
        World testWorld = World.generateNewWorld(x, z, false);
        Tile[,] worldArray = World.getWorld().getWorldArray();
        for (int i = 0; i < x; i++)
        {
            for(int j = 0; j < z; j++)
            {
                assertBetween(worldArray[i, j].getLocalWater().getFlowRateMultiplier(), 0.0, 1.0);
                assertBetween(worldArray[i, j].getLocalWater().getSoilAbsorption(), 0.0, 1.2);
            }
        }
    }
    
    // make another test that prints the results of a 10x10 world downstream and upstream values
    [Test]
    public void visualCompareUpDownstream()
    {
        World worldTest = World.generateNewWorld(10, 10, false);
        Tile[,] worldArray = worldTest.getWorldArray();
        double[,] elevationArray = new double[10, 10];
        string[,] downArray = new string[10, 10];
        List<string>[,] upArray = new List<string>[10, 10];
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                elevationArray[i, j] = worldArray[i, j].getElevation();
                downArray[i, j] = worldArray[i, j].getLocalWater().getDownstreamDirection();
                upArray[i, j] = worldArray[i, j].getLocalWater().getUpstreamDirections();
            }
        }
        ArrayPrinter.print(elevationArray, "Elevation Map: ");
        ArrayPrinter.print(downArray, "Downstream Map:");
        ArrayPrinter.print(upArray, "Upstream Map:");
    }

    private void assertBetween(double num, double v1, double v2)
    {
        Assert.LessOrEqual(num, v2);
        Assert.GreaterOrEqual(num, v1);
    }

}
