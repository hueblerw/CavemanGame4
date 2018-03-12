using NUnit.Framework;
using System.Collections.Generic;

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
                        UnityEngine.Vector2 coor = Support.directionToCoor(reverseDirection, i, j);
                        Assert.AreEqual(reverseDirection, worldArray[i, j].getLocalWater().getDownstreamDirection());
                    }
                }
            }
        }
        Assert.AreEqual(true, false);
	}

    [Test]
    public void DownstreamIsHigherThanTarget()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual(true, false);
    }

    [Test]
    public void IfLakeAllSurroundingAreHigher()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual(true, false);
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
                assertBetween(worldArray[i, j].getLocalWater().getSoilAbsorption(), 0.0, 1.0);
            }
        }
    }

    private void assertBetween(double num, double v1, double v2)
    {
        Assert.LessOrEqual(num, v2);
        Assert.GreaterOrEqual(num, v1);
    }

}
