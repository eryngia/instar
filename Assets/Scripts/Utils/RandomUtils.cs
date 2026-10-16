using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Random util functions affected by seed.
public static class RandomUtils {

    // Returns random float in range biased towards the lower end.
    public static float RangeLow(float min, float max, float bias = 2f) {
        float t = Random.Range(0f, 1f);                         
        t = Mathf.Pow(t, bias);                                
        return Mathf.Lerp(min, max, t);                        
    }

    // Returns random float in range biased towards higher end.
    public static float RangeHigh(float min, float max, float bias = 2f) {
        float t = Random.Range(0f, 1f);                         
        t = Mathf.Pow(t, 1f / bias);                            
        return Mathf.Lerp(min, max, t);                         
    }

    // Returns random float in range biased towards average.
    public static float RangeAvg(float min, float max, int count = 2) {
        return Enumerable.Range(0, count).Select(_ => Random.Range(min, max)).Average();
    }

    // Returns ranfom integer in range biased towards average.
    public static int RangeAvgInt(int min, int max, int count = 2) {
        return (int) Enumerable.Range(0, count).Select(_ => Random.Range(min, max)).Average();
    }

    // Divides a number into count array elements randomly.
    public static int[] Divide(int num, int count) {
        int[] result = new int[count];
        for (int i = 0; i < num; i++) {
            result[Random.Range(0, count - 1)]++;
        }
        return result;
    }

    // Returns a random element of a list.
    public static T FromList<T>(List<T> list) {
        return list[Random.Range(0, list.Count)];
    }

    // Returns a random element of an array.
    public static T FromArray<T>(T[] array) {
        return array[Random.Range(0, array.Length)];
    }

    // Returns one of three values based on given weights.
    public static T Weighted<T>(T value1, float weight1, T value2, float weight2, T value3, float weight3) {
        float totalWeight = weight1 + weight2 + weight3;
        float randomPoint = Random.Range(0f, totalWeight);

        if (randomPoint < weight1)
            return value1;
        else if (randomPoint < weight1 + weight2)
            return value2;
        else
            return value3;
    }

}