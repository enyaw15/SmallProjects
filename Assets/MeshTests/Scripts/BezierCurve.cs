using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
This is a Quadradic Bezier curve
From Wikipedia
A quadratic Bézier curve is the path traced by the function B(t), given points P0, P1, and P2,
B(t) = (1-t)((1-t)P0+t*P1) + t((1-t)P1+ t*p2) for 0<t<1 //the standard definition
  
which can be interpreted as the linear interpolant of corresponding points on the linear Bézier curves from P0 to P1 and from P1 to P2 respectively. Rearranging the preceding equation yields:
B(t) = (1-t)^2*P0 + 2(1-t)t*P1 + t^2*p2  for 0<t<1 //each point is in a seperate term
  
This can be written in a way that highlights the symmetry with respect to P1:
B(t) P1 + (1-t)^2(P0-P1) +t^2(P2-P1)
  
Which immediately gives the derivative of the Bézier curve with respect to t:
B'(t) = 2(1-t)(P0-P1) +2t(P2-P1)
from which it can be concluded that the tangents to the curve at P0 and P2 intersect at P1.
 As t increases from 0 to 1, the curve departs from P0 in the direction of P1, then bends to arrive at P2 from the direction of P1.
The second derivative of the Bézier curve with respect to t is
B''(t) = 2(p2-2p1+p0)
*/

//holds the data of the bezier curve
public class BezierCurve : MonoBehaviour {

    public Vector3[] points;

    public Vector3 getPoint(float t)
    {
        return transform.TransformPoint(Bezier.getPoint(points[0], points[1], points[2],points[3], t));
    }

    public void Reset()
    {
        points = new Vector3[] {
            new Vector3(1f, 0f, 0f),
            new Vector3(2f, 0f, 0f),
            new Vector3(3f, 0f, 0f),
            new Vector3(4f, 0f, 0f)
        };
    }
    public Vector3 getDirection(float t)
    {
        return transform.TransformPoint(Bezier.getFirstDerivative(points[0], points[1], points[2],points[3], t)) - transform.position;
    }
}

//contains the math for quadratic and cubic bezier curves
public static class Bezier
{
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
        t = Mathf.Clamp01(t);// for 0<t<1
        //B'(t) = 2(1-t)(P0-P1) +2t(P2-P1)
        return 2f * (1f - t) * (p1 - p0) + 2f * t * (p2 - p1);
    }

    //returs a point on the a cubic bezier curve
    public static Vector3 getPoint (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
		t = Mathf.Clamp01(t);// for 0<t<1
		float oneMinusT = 1f - t;
		return oneMinusT * oneMinusT * oneMinusT * p0 + 3f * oneMinusT * oneMinusT * t * p1 + 3f * oneMinusT * t * t * p2 + t * t * t * p3;
	}
	//returns the derivative at a point on a bezier curve
	public static Vector3 getFirstDerivative (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
		t = Mathf.Clamp01(t);// for 0<t<1
		float oneMinusT = 1f - t;
		return 3f * oneMinusT * oneMinusT * (p1 - p0) + 6f * oneMinusT * t * (p2 - p1) + 3f * t * t * (p3 - p2);
	}

}
