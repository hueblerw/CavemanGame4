using NUnit.Framework;
using System.Diagnostics;
using System.Text.RegularExpressions;

public class TileTests {

    [Test]
    public void ElevationMapTest()
    {
        // Use the Assert class to test conditions.
        int x = 40;
        int z = 40;
        World testWorld = World.generateNewWorld(x, z, false);
        Tile[,] worldArray = World.getWorld().getWorldArray();
        double[,] array = new double[x, z];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < z; j++)
            {
                array[i, j] = worldArray[i, j].getElevation();
                assertBetween(array[i, j], -100.0, 100.0);
                assertHasBeenRoundedToXDecimals(array[i, j], 2);
            }
        }
        ArrayPrinter.print(array, "Elevation Map:");
    }

    [Test]
    public void HighTempTest()
    {
        // Use the Assert class to test conditions.
        int x = 40;
        int z = 40;
        World testWorld = World.generateNewWorld(x, z, false);
        Tile[,] worldArray = World.getWorld().getWorldArray();
        int[,] array = new int[x, z];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < z; j++)
            {
                array[i, j] = worldArray[i, j].getHighTemp();
                assertBetween(array[i, j], 20, 100);
                Assert.LessOrEqual(array[i, j].ToString().Length, 3);
            }
        }
        ArrayPrinter.print(array, "High Temp Map:");
        // VerifyWorld();
    }

    [Test]
    public void LowTempTest()
    {
        // Use the Assert class to test conditions.
        int x = 40;
        int z = 40;
        World testWorld = World.generateNewWorld(x, z, false);
        Tile[,] worldArray = World.getWorld().getWorldArray();
        int[,] array = new int[x, z];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < z; j++)
            {

                array[i, j] = worldArray[i, j].getLowTemp();
                assertBetween(array[i, j], -20, 70);
                Assert.LessOrEqual(array[i, j].ToString().Length, 3);
            }
        }
        ArrayPrinter.print(array, "Low Temp Map:");
        // VerifyWorld();
    }

    [Test]
    public void MidpointTest()
    {
        // Use the Assert class to test conditions.
        int x = 40;
        int z = 40;
        World testWorld = World.generateNewWorld(x, z, false);
        Tile[,] worldArray = World.getWorld().getWorldArray();
        double[,] array = new double[x, z];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < z; j++)
            {
                array[i, j] = worldArray[i, j].getMidpoint();
                assertBetween(array[i, j], 45.0, 75.0);
                assertHasBeenRoundedToXDecimals(array[i, j], 1);
            }
        }
        ArrayPrinter.print(array, "Midpoint Map:");
        // VerifyWorld();
    }

    [Test]
    public void VarianceTest()
    {
        // Use the Assert class to test conditions.
        int x = 40;
        int z = 40;
        World testWorld = World.generateNewWorld(x, z, false);
        Tile[,] worldArray = World.getWorld().getWorldArray();
        double[,] array = new double[x, z];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < z; j++)
            {
                array[i, j] = worldArray[i, j].getVariance();
                assertBetween(array[i, j], 0.0, 12.0);
                assertHasBeenRoundedToXDecimals(array[i, j], 2);
            }
        }
        ArrayPrinter.print(array, "Variance Map:");
        // VerifyWorld();
    }

    [Test]
    public void HumidityTests()
    {
        // Use the Assert class to test conditions.
        int x = 40;
        int z = 40;
        World testWorld = World.generateNewWorld(x, z, false);
        Tile[,] worldArray = World.getWorld().getWorldArray();
        Humidity[,] array = new Humidity[x, z];
        double[,] example = new double[x, z];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < z; j++)
            {
                array[i, j] = worldArray[i, j].getHumidity();
                for (int k = 0; k < array[i, j].getSegments().Length; k++)
                {
                    if (k == 4)
                    {
                        example[i, j] = array[i, j].getSegments()[k];
                    }
                    assertBetween(array[i, j].getSegments()[k], 0.0, 12.0);
                    assertHasBeenRoundedToXDecimals(array[i, j].getSegments()[k], 2);
                }
            }
        }
        ArrayPrinter.print(example, "Sample Humidity Map:");
        // VerifyWorld();
    }

    [Test]
    public void generateYearOfTempsTest()
    {
        int x = 40;
        int z = 40;
        World testWorld = World.generateNewWorld(x, z, false);
        Tile tile = World.getWorld().getWorldArray()[0, 0];
        Stopwatch sw = Stopwatch.StartNew();
        sw.Start();
        tile.generateYearOfTemps();
        sw.Stop();
        UnityEngine.Debug.Log("Generation of a year of temperatures " + World.X + ", " + World.Z + " took " + sw.Elapsed + " secs.");
        UnityEngine.Debug.Log("Note: World Size should have no effect on the time run of this method.");
    }

    [Test]
    public void generateYearOfRainAndSnowTest()
    {
        int x = 40;
        int z = 40;
        World testWorld = World.generateNewWorld(x, z, false);
        Tile tile = World.getWorld().getWorldArray()[0, 0];
        Stopwatch sw = Stopwatch.StartNew();
        sw.Start();
        tile.generateYearOfRainAndSnow();
        sw.Stop();
        UnityEngine.Debug.Log("Generation of a year of temperatures " + World.X + ", " + World.Z + " took " + sw.Elapsed + " secs.");
        UnityEngine.Debug.Log("Note: World Size will have an effect on the time run of this method.");
    }

    [Test]
    public void generateWorldsYearOfTempsTest()
    {
        // syncronously first, but theoretically all river initialization can be down asyncronously if necessary
        int x = 40;
        int z = 40;
        World testWorld = World.generateNewWorld(x, z, false);
        Tile[,] worldArray = World.getWorld().getWorldArray();
        Stopwatch sw = Stopwatch.StartNew();
        sw.Start();
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < z; j++)
            {
                worldArray[i, j].generateYearOfTemps();
            }
        }
        sw.Stop();
        UnityEngine.Debug.Log("Generation of an entire year's worth of temperatures " + World.X + ", " + World.Z + " took " + sw.Elapsed + " secs.");
        UnityEngine.Debug.Log("Note: World Size will have an effect on the time run of this method.");
    }

    private void assertBetween(double num, double v1, double v2)
    {
        Assert.LessOrEqual(num, v2);
        Assert.GreaterOrEqual(num, v1);
    }

    private void assertHasBeenRoundedToXDecimals(double num, int x)
    {
        string numAsString = num.ToString();
        if (numAsString.Contains("."))
        {
            numAsString = Regex.Replace(numAsString, "-{0,1}[0-9]*\\.", "");
            Assert.LessOrEqual(numAsString.Length, 2);
        }
    }

}
