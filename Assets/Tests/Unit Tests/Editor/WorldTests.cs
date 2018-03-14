using NUnit.Framework;
using System.Diagnostics;

public class WorldTests {

    [Test]
	public void GenerateAsymmetricWorld() {
        // Use the Assert class to test conditions.
        int x = 50;
        int z = 40;
        Stopwatch sw = Stopwatch.StartNew();
        sw.Start();
        World testWorld = World.generateNewWorld(x, z, false);
        sw.Stop();
        UnityEngine.Debug.Log("World of size " + x + ", " + z + " took " + sw.Elapsed + " secs.");
        // VerifyWorld();
    }

    [Test]
    public void GenerateWorldLayer()
    {
        // Use the Assert class to test conditions.
        int x = 50;
        int z = 40;
        UnityEngine.Debug.Log("World Size: " + World.X + ", " + World.Z);
        Stopwatch sw = Stopwatch.StartNew();
        sw.Start();
        double[,] layer = World.getWorld().generateWorldLayer(-20.0, 40.0, 2.0, 2.5, true);
        sw.Stop();
        UnityEngine.Debug.Log("Layer generation of size " + World.X + ", " + World.Z + " took " + sw.Elapsed + " secs.");
        verifyLayer(layer);
        ArrayPrinter.print(layer, "Test Layer: ");
    }

    [Test]
    public void GenerateSmallSymmetricWorld()
    {
        // Use the Assert class to test conditions.
        int x = 50;
        int z = 50;
        Stopwatch sw = Stopwatch.StartNew();
        sw.Start();
        World testWorld = World.generateNewWorld(x, z, false);
        sw.Stop();
        UnityEngine.Debug.Log("World of size " + x + ", " + z + " took " + sw.Elapsed + " secs.");

        // VerifyWorld();
    }

    [Test]
    public void GenerateMediumSymmetricWorld()
    {
        // Use the Assert class to test conditions.
        int x = 200;
        int z = 200;
        Stopwatch sw = Stopwatch.StartNew();
        sw.Start();
        World testWorld = World.generateNewWorld(x, z, false);
        sw.Stop();
        UnityEngine.Debug.Log("World of size " + x + ", " + z + " took " + sw.Elapsed + " secs.");
    }

    [Test]
    public void GenerateLargeSymmetricWorld()
    {
        // Use the Assert class to test conditions.
        int x = 400;
        int z = 400;
        Stopwatch sw = Stopwatch.StartNew();
        sw.Start();
        World testWorld = World.generateNewWorld(x, z, false);
        sw.Stop();
        UnityEngine.Debug.Log("World of size " + x + ", " + z + " took " + sw.Elapsed + " secs.");
    }
    
    // move this higher up
    [Test]
    public void negativeElevationHasSomeOcean()
    {
        Tile[,] array = World.getWorld().getWorldArray();
        for (int i = 0; i < x; i++){
            for (int j = 0; j < z; j++){
                if(arrray[i,j].getElevation() < 0.0)){
                    Assert.LessThan(array[i,j]. getElevation(), 0.0);
                }
            }
        }
    }

    // PRIVATE METHODS

    private void VerifyWorld(World world)
    {
        // make sure that the world is generated correctly.
        // This is only to be done for small worlds to save test time.
    }

    private void verifyLayer(double[,] layer)
    {
        // Check world is the expected size.
        Assert.AreEqual(World.X, layer.GetLength(0));
        Assert.AreEqual(World.Z, layer.GetLength(1));

        Assert.AreEqual(layer[0, 0], 2.5);
        for (int i = 0; i < layer.GetLength(0); i++)
        {
            for (int j = 0; j < layer.GetLength(1); j++)
            {
                Assert.GreaterOrEqual(layer[i, j], -20.0);
                Assert.LessOrEqual(layer[i, j], 40.0);
            }
        }
    }

}
