using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(EnemyPath))]
public class WaypointEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EnemyPath myScript = (EnemyPath)target;

        if (GUILayout.Button("Add Waypoint"))
        {
            myScript.AddWayPoint();
        }

        DrawDefaultInspector();
    }
}
#endif
