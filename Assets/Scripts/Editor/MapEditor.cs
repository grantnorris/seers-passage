using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BuildMap))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        BuildMap buildMap = (BuildMap)target;

        if (GUILayout.Button("Build Map")) {
            buildMap.Build();
        }

        if (GUILayout.Button("Remove Map")) {
            buildMap.Remove();
        }
    }
}
