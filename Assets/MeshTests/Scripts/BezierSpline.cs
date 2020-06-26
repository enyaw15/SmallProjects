using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//holds the data of the bezier spline
public class BezierSpline : MonoBehaviour {

    [SerializeField]
	private Vector3[] points;
    //returns the number of curves
    public int curveCount {
		get {
			return (points.Length - 1) / 3;
		}
	}

    public int controlPointCount {
		get {
			return points.Length;
		}
	}

	public Vector3 getControlPoint (int index) {
		return points[index];
	}

	public void setControlPoint (int index, Vector3 point) {
		points[index] = point;
	}

    //when the reset option is used in the inspector default values are applied
    public void Reset()
    {
        points = new Vector3[] {
            new Vector3(1f, 0f, 0f),
            new Vector3(2f, 0f, 0f),
            new Vector3(3f, 0f, 0f),
            new Vector3(4f, 0f, 0f)
        };
    }

    //returns a point along the spline
    public Vector3 getPoint(float t)
    {
        int i;
		if (t >= 1f)
        {
			t = 1f;//the highest value
			i = points.Length - 4;//the last starting value
		}
		else {
			t = Mathf.Clamp01(t) * curveCount;//scale t so that it is 0 to 1 value where 0 and 1 are the ends of the spline
			i = (int)t;//i is the whole value of t aka the current curve we are on
			t -= i;//remove the current curve to get the distance along that curve
			i *= 3;//multiply by three to go from current curve to the start point of that curve
		}
        return transform.TransformPoint(Bezier.getPoint(points[i], points[i+1], points[i+2],points[i+3], t));
    }

    //returns the velocity at a point along the spline
    public Vector3 getDirection(float t)
    {
        int i;
		if (t >= 1f)
        {
			t = 1f;//the highest value
			i = points.Length - 4;//the last starting value
		}
		else {
			t = Mathf.Clamp01(t) * curveCount;//scale t so that it is 0 to 1 value where 0 and 1 are the ends of the spline
			i = (int)t;//i is the whole value of t aka the current curve we are on
			t -= i;//remove the current curve to get the distance along that curve
			i *= 3;//multiply by three to go from current curve to the start point of that curve
		}
        return transform.TransformPoint(Bezier.getFirstDerivative(points[i], points[i+1], points[i+2],points[i+3], t)) - transform.position;
    }

    //adds a new curve to the spline
    public void AddCurve () {
		Vector3 point = points[points.Length - 1];
		Array.Resize(ref points, points.Length + 3);
		point.x += 1f;
		points[points.Length - 3] = point;
		point.x += 1f;
		points[points.Length - 2] = point;
		point.x += 1f;
		points[points.Length - 1] = point;
	}
}