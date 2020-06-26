using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Track
{
    [SerializeField]
    public Vector3[] points;

    public Track(Vector3 start, Vector3 bPoint, Vector3 end)
    {
        points = new Vector3[3];
        points[0] = start;
        points[1] = bPoint;
        points[2] = end;
    }

    public Vector3 GetPoint(float t)
    {
        return (Bezier.GetPoint(points[0], points[1], points[2], t));
    }

    public Vector3 GetVelocity(float t)
    {
        return (Bezier.GetFirstDerivative(points[0], points[1], points[2], t));
    }

}
