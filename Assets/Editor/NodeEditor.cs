using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Node))]
[CanEditMultipleObjects]
public class NodeEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Get Node Neighbours"))
        {
            foreach(Node obj in targets)
                obj.FindNeighbours();

            Debug.Log("Finished building node neighbours");
        }
    }

}
