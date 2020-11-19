#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(PositionInspector))]
public class PositionInspectorEditor : Editor{
	public override void OnInspectorGUI()
	{
		PositionInspector inspector = (PositionInspector)target;
		EditorGUILayout.Vector3Field("LocalPosition", inspector.LocalPosition);
		EditorGUILayout.Vector3Field("GlobalPosition", inspector.GlobalPosition);
	}
}

#endif