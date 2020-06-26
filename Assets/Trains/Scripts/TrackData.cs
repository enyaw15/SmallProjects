using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TrackData
{
    [SerializeField]
    public Vector3[] points;

    public TrackData(Vector3 start, Vector3 bPoint, Vector3 end)
    {
        //creates the quadratic bezier curve
        points = new Vector3[3];
        points[0] = start;
        points[1] = bPoint;
        points[2] = end;
    }

    public Vector3 getPoint(float t)
    {
        return (Bezier.getPoint(points[0], points[1], points[2], t));
    }

    public Vector3 getVelocity(float t)
    {
        return (getFirstDerivative(points[0], points[1], points[2], t));
    }
    
    //gives a point from the functoin B(t) as defined in the header comment
    public static Vector3 getPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        t = Mathf.Clamp01(t);// for 0<t<1
        float oneMinusT = 1f - t;
        //B(t) =      (1-t)^2        * P0   + 2     (1-t)      t * P1 + t^2   * p2  for 0<t<1 //each point is in a seperate term
        return oneMinusT * oneMinusT * p0   + 2f * oneMinusT * t * p1 + t * t * p2;
    }

    //returns the first derivative of the function B(t). B'(t) as definded in the header
    public static Vector3 getFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        //B'(t) = 2(1-t)(P0-P1) +2t(P2-P1)
        return 2f * (1f - t) * (p1 - p0) + 2f * t * (p2 - p1);
    }

}
