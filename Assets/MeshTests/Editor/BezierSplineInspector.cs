using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierSpline))]
public class BezierSplineInspector : Editor
{

    private BezierSpline spline;
    private Transform handleTransform;
    private Quaternion handleRotation;
    private const float directionScale = 0.2f;

    private const int stepsPerCurve = 10;

    private const float handleSize = 0.04f;
	private const float pickSize = 0.06f;
	private int selectedIndex = -1;

    private void OnSceneGUI()
    {
        spline = target as BezierSpline;//the bezier spline data
        handleTransform = spline.transform;//the transform of the handle is the transform of the spline
        handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;//ensures that the rotation is in world space

        for(int i = 0; i < spline.controlPointCount-1;i+=3)
        {
            //the points that define the spline
            Vector3 p0 = ShowPoint(i);
            Vector3 p1 = ShowPoint(i+1);
            Vector3 p2 = ShowPoint(i+2);
            Vector3 p3 = ShowPoint(i+3);

            //draws a red line between the points
            //one line is covered by the tangent lines since tangent lines from the endpoints always intersect at the mid point
            Handles.color = Color.red;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p2, p3);

            //draw the spline by drawing lines between evenly spaced points on the spline
            Handles.color = Color.white;//color for the spline
            Vector3 lineStart = spline.getPoint(0f);//the line starts at B(0)
            ShowDirections();
            Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
        }
    }

    //create a custom inspetor gui
    public override void OnInspectorGUI () {
		//DrawDefaultInspector();//do the defualt stuff
		spline = target as BezierSpline;//the spline
		if (selectedIndex >= 0 && selectedIndex < spline.controlPointCount) {
			DrawSelectedPointInspector();
		}
		if (GUILayout.Button("Add Curve")) {//if the button add curve is clicked
			Undo.RecordObject(spline, "Add Curve");
			spline.AddCurve();//add a curve
			EditorUtility.SetDirty(spline);
		}
	}

	private void DrawSelectedPointInspector() {
		GUILayout.Label("Selected Point");
		EditorGUI.BeginChangeCheck();
		Vector3 point = EditorGUILayout.Vector3Field("Position", spline.getControlPoint(selectedIndex));
		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(spline, "Move Point");
			EditorUtility.SetDirty(spline);
			spline.setControlPoint(selectedIndex, point);
		}
	}

    private void ShowDirections () {
		Handles.color = Color.green;
		Vector3 point = spline.getPoint(0f);
		Handles.DrawLine(point, point + spline.getDirection(0f) * directionScale);
		int steps = stepsPerCurve * spline.curveCount;
		for (int i = 1; i <= steps; i++) {
			point = spline.getPoint(i / (float)steps);
			Handles.DrawLine(point, point + spline.getDirection(i / (float)steps) * directionScale);
		}
	}

    
    private Vector3 ShowPoint(int index)
    {
        Vector3 point = handleTransform.TransformPoint(spline.getControlPoint(index));//get the point
        float size = HandleUtility.GetHandleSize(point);//how large the point should be based on screen size
        Handles.color = Color.white;
		if (Handles.Button(point, handleRotation, size * handleSize, size * pickSize, Handles.DotCap))//if the button is pressed
        {
			selectedIndex = index;
			Repaint();//redraw the inspector when a new point is selected
		}
		if (selectedIndex == index) {
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(spline, "Move Point");
                EditorUtility.SetDirty(spline);
                spline.setControlPoint(index, handleTransform.InverseTransformPoint(point));
            }
        }
        return point;
    }
}