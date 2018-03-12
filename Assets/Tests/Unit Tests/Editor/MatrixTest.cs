using NUnit.Framework;
using System.Diagnostics;

public class MatrixTest {

	[Test]
	public void TimeTheTempEqCreation() {
        Stopwatch sw = Stopwatch.StartNew();
        sw.Start();
        TempEquation tempEq = new TempEquation(70, 20, 56, 6.0);
        sw.Stop();
        UnityEngine.Debug.Log("Temp Eq constructor took " + sw.Elapsed + " secs to run.");
    }

    [Test]
    public void TimeTheTempEqReturnDaysTemp()
    {
        TempEquation tempEq = new TempEquation(70, 20, 56, 6.0);
        Stopwatch sw = Stopwatch.StartNew();
        sw.Start();
        tempEq.generateTodaysTemp(45, new System.Random());
        sw.Stop();
        UnityEngine.Debug.Log("Temp Eq generateTodaysTemp took " + sw.Elapsed + " secs to run.");
    }

}
