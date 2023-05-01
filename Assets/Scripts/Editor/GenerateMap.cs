using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Map))]
public class GenerateMap : Editor
{   
    Map map;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        map = (Map)target;

        if (GUILayout.Button("Build Map")) {
            map.Build();
        }

        if (GUILayout.Button("Remove Map")) {
            map.Remove();
        }
    }
}
