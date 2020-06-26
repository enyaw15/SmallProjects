using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BezierControlPointMode {
	Free,
	Aligned,
	Mirrored
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