using System;
using UnityEngine;

public class ArrayPrinter {

	public static void print(double[,] array, string titleMessage)
    {
        Debug.Log(titleMessage + "\n\n");
        string output = "";
        for(int i = 0; i < array.GetLength(0); i++)
        {
            for(int j = 0; j < array.GetLength(1); j++)
            {
                output += array[i, j].ToString() + " ";
            }
            output += "\n";
        }
        Debug.Log(output);
    }

    public static void print(int[,] array, string titleMessage)
    {
        Debug.Log(titleMessage + "\n\n");
        string output = "";
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                output += array[i, j].ToString() + " ";
            }
            output += "\n";
        }
        Debug.Log(output);
    }

    public static void print(double[] array, string titleMessage)
    {
        Debug.Log(titleMessage + "\n\n");
        string output = "";
        for (int j = 0; j < array.GetLength(1); j++)
        {
            output += array[j].ToString() + " ";
        }
        Debug.Log(output);
    }

}
