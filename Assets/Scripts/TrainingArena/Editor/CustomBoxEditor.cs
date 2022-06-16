using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(CustomBox))]
public class CustomBoxEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		
		CustomBox cb = (CustomBox)target;
		if(GUILayout.Button("Create")){
			cb.Create();
		}
		
		if(GUILayout.Button("Reset")){
			cb.Reset();
		}
	}	
}
