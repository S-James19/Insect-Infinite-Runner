using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distance : MonoBehaviour
{
    /// <summary>
    /// Function to calculate the distance between two vector points
    /// </summary>
    /// <param name="point1"> The point furthest away </param>
    /// <param name="point2"> The current point </param>
    /// <returns> a float distance between points </returns>
    public static float DistanceBetweenPoints(Vector3 point1, Vector3 point2)
    {
        Vector3 vectorDistance = point1 - point2;
        float distance = Mathf.Sqrt((Mathf.Pow(vectorDistance.x, 2)) + (Mathf.Pow(vectorDistance.y, 2)) + (Mathf.Pow(vectorDistance.z, 2)));
        return distance;
    }

    /// <summary>
    /// Function to calculate the distance of a single vector
    /// </summary>
    /// <param name="point"> The vector point </param>
    /// <returns> a float distance of vector point </returns>
    public static float CalcHypotenuse(Vector3 point)
    {
        return Mathf.Sqrt((Mathf.Pow(point.x, 2)) + (Mathf.Pow(point.y, 2)) + (Mathf.Pow(point.z, 2)));
    }
}
