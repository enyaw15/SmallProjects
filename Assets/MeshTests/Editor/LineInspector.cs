using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ThisLine))]
public class LineInspector : Editor
{
    private void OnSceneGUI()
    {
        ThisLine line = target as ThisLine;

        Handles.color = Color.white;
        Transform handleTransform = line.transform;
        Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ?
        handleTransform.rotation : Quaternion.identity;
        Vector3 p0 = handleTransform.TransformPoint(line.PointOne);
        Vector3 p1 = handleTransform.TransformPoint(line.PointTwo);

        Handles.color = Color.white;
        Handles.DrawLine(p0, p1);
        Handles.DoPositionHandle(p0, handleRotation);
        Handles.DoPositionHandle(p1, handleRotation);

        EditorGUI.BeginChangeCheck();
        p0 = Handles.DoPositionHandle(p0, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(line, "Move Point");
            EditorUtility.SetDirty(line);
            line.PointOne = handleTransform.InverseTransformPoint(p0);
        }
        EditorGUI.BeginChangeCheck();
        p1 = Handles.DoPositionHandle(p1, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(line, "Move Point");
            EditorUtility.SetDirty(line);
            line.PointTwo = handleTransform.InverseTransformPoint(p1);
        }

    }

}