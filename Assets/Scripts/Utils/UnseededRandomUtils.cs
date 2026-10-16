using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

// Random util functions that do not affect the seed.
public static class UnseededRandomUtils {

    // Return a random integer between min (inclusive) and max (exclusive).
    public static int Range(int min, int max) {
        return new Random().Next(min, max);
    }

    // Return a random float between min (inclusive) and max (exclusive).
    public static float Range(float min, float max) {
        return (float) (new Random().NextDouble() * (max - min) + min);
    }

    // Return a random element of a list.
    public static T FromList<T>(List<T> list) {
        return list[Range(0, list.Count)];
    }
    
    // Return a random element of an array.
    public static T FromArray<T>(T[] array) {
        return array[Range(0, array.Length)];
    }

    // Return a random point inside the unit circle.
    public static Vector2 InsideUnitCircle() {
        Random rnd = new Random();
        double theta = rnd.NextDouble() * 2 * Math.PI; 
        double radius = Math.Sqrt(rnd.NextDouble());  

        float x = (float)(radius * Math.Cos(theta));
        float y = (float)(radius * Math.Sin(theta));

        return new Vector2(x, y);
    }
   

}