using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class q : MonoBehaviour
{
    public static int getRandI(int min, int max)
    {
        return (int) Random.Range(min, max);
    }

    public static int getRandI(float min, float max)
    {
        return (int) Random.Range(min, max);
    }

    public static float getRandF(float min, float max)
    {
        return Random.Range(min, max);
    }

    public static float dist(Vector3 a, Vector3 b)
    {
        float x = a.x - b.x;
        float y = a.y - b.y;

        return Mathf.Sqrt(x * x + y * y);
    }

    public static void print(string str)
    {
        Debug.Log(str);
    }

    public static bool inRangeI(int min, int max, int num)
    {
        return num >= min && num <= max;
    }

    public static bool inRangeF(float min, float max, float num)
    {
        return num >= min && num <= max;
    }

    public static bool approx(float num, float a)
    {
        float sub = Mathf.Abs(num - 0.5f);
        return inRangeF(-sub, sub, a);
    }
}
