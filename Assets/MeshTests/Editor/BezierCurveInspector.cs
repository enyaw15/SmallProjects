using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierCurve))]
public class BezierCurveInspector : Editor
{

    private BezierCurve curve;
    private Transform handleTransform;
    private Quaternion handleRotation;
    private const float directionScale = 0.2f;

    //defines how many points will be used to make the line
    private const int lineSteps = 10;

    private void OnSceneGUI()
    {
        curve = target as BezierCurve;//the bezier curve data
        handleTransform = curve.transform;//the transform of the handle is the transform of the curve
        handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;//ensures that the rotation is in world space

        //the points that define the curve
        Vector3 p0 = ShowPoint(0);
        Vector3 p1 = ShowPoint(1);
        Vector3 p2 = ShowPoint(2);
        Vector3 p3 = ShowPoint(3);

        //draws a red line between the points
        //one line is covered by the tangent lines since tangent lines from the endpoints always intersect at the mid point
        Handles.color = Color.red;
        Handles.DrawLine(p0, p1);
        Handles.DrawLine(p2, p3);

        //draw the curve by drawing lines between evenly spaced points on the curve
        Handles.color = Color.white;//color for the curve
        Vector3 lineStart = curve.getPoint(0f);//the line starts at B(0)
        ShowDirections();
		Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
        //draw the curve by drawing lines between evenly spaced points on the curve
        // for (int i = 1; i <= lineSteps; i++)
        // {
        //     Vector3 lineEnd = curve.getPoint(i / (float)lineSteps);
        //     Handles.color = Color.white;
        //     Handles.DrawLine(lineStart, lineEnd);
        //     lineStart = lineEnd;
        // }
    }

    private void ShowDirections () {
		Handles.color = Color.green;
		Vector3 point = curve.getPoint(0f);
		Handles.DrawLine(point, point + curve.getDirection(0f) * directionScale);
		for (int i = 1; i <= lineSteps; i++) {
			point = curve.getPoint(i / (float)lineSteps);
			Handles.DrawLine(point, point + curve.getDirection(i / (float)lineSteps) * directionScale);
		}
	}

    
    private Vector3 ShowPoint(int index)
    {
        Vector3 point = handleTransform.TransformPoint(curve.points[index]);
        EditorGUI.BeginChangeCheck();
        point = Handles.DoPositionHandle(point, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curve, "Move Point");
            EditorUtility.SetDirty(curve);
            curve.points[index] = handleTransform.InverseTransformPoint(point);
        }
        return point;
    }
}